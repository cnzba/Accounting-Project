using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService service;
        private readonly ILogger logger;

        public InvoiceController(IInvoiceService service, ILoggerFactory loggerFactory)
        {
            this.service = service;
            logger = loggerFactory.CreateLogger<InvoiceController>();
        }

        // GET: api/invoice
        [HttpGet]
        public IActionResult GetInvoice()
        {
            var invoices = service.GetInvoiceHeaders();

            if (invoices == null)
            {
                return NotFound();
            }

            return Ok(invoices);
        }

        // GET api/invoice/5
        [HttpGet("{InvoiceNumber}")]
        public IActionResult GetInvoice([FromRoute] string invoiceNumber)
        {
            var invoice = service.GetInvoice(invoiceNumber);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        // GET api/invoice/p/5 for payment without auth
        [Route("p/{PaymentId}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetInvoiceNoAuth([FromRoute] string paymentId)
        {
            var invoice = service.GetInvoiceByPaymentId(paymentId);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        // POST: api/invoice
        [HttpPost]
        public IActionResult CreateInvoice([FromBody] DraftInvoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Invoice created = service.CreateInvoice(invoice);
                if (created!=null)
                    return CreatedAtAction("GetInvoice", new { InvoiceNumber = created.InvoiceNumber }, created);
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return BadRequest("Failed to create new invoice");
        }

        // PUT: api/invoice/5
        [HttpPut("{InvoiceNumber}")]
        public IActionResult ModifyInvoice([FromRoute] string invoiceNumber,
            [FromBody] DraftInvoice invoice)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            if (invoiceNumber != invoice.InvoiceNumber)
            {
                return BadRequest("Unexpected invoice number");
            }

            try
            {
                if (!service.ModifyInvoice(invoice))
                {
                    return BadRequest("Unable to modify invoice.");
                }
                else return Ok(service.GetInvoice(invoice.InvoiceNumber));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (!service.InvoiceExists(invoiceNumber)) return NotFound();

                logger.LogError(ex.Message);
                return BadRequest("Unable to modify invoice.");
            }
        }

        // DELETE api/invoice/5
        [HttpDelete("{InvoiceNumber}")]
        public IActionResult DeleteInvoice([FromRoute] string invoiceNumber)
        {
            if (!service.InvoiceExists(invoiceNumber)) return NotFound();

            try
            {
                if (service.DeleteInvoice(invoiceNumber)) return Ok("Deletion of draft invoice successful.");
                else return BadRequest("Unable to delete invoice.");
            }
            catch (ArgumentException)
            {
                return BadRequest("Invoice had status other than draft.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest("Unable to delete invoice.");
            }
        }

        [Route("s")]
        [HttpGet]
        public IActionResult SearchByKeywordAndSort([FromQuery] string keyword, [FromQuery] string sort)
        {
            var invoices = service.GetAllInvoicesBy(keyword, sort);

            if (invoices == null)
            {
                return NotFound();
            }

            return Ok(invoices);
        }

        [Route("invoicenumber")]
        [HttpGet]
        public IActionResult GetNewInvoiceNumber([FromQuery] string login)
        {
            string invoiceNumber = service.GenerateOrganisationInvoiceNumber(login);

            if (invoiceNumber == null)
            {
                return NotFound();
            }
            
            return Ok(invoiceNumber);
        }
    }
}
