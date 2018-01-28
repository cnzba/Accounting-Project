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
                if (service.CreateInvoice(invoice))
                    return CreatedAtAction("GetInvoice", new { InvoiceNumber = invoice.InvoiceNumber }, invoice);
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
                return BadRequest();
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
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
