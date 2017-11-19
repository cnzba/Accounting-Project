using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
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
            var invoices = service.GetAllInvoices();

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
        public IActionResult CreateInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (service.CreateInvoice(invoice))
                    return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceNumber }, invoice);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return BadRequest("Failed to save create invoice");
        }

        // PUT: api/invoice/5
        [HttpPut("{id}")]
        public IActionResult ModifyInvoice([FromRoute] string invoiceNumber, [FromBody] Invoice invoice)
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
                else return Ok(invoice);
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
