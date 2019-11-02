using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AutoMapper;
using WebApp.Models;
using WebApp.Services;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService service;
        private readonly IMapper mapper;
        private readonly ILogger<InvoiceController> logger;
        public InvoiceController(IInvoiceService service, IMapper mapper, ILogger<InvoiceController> logger)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/invoice
        [HttpGet]
        public ActionResult<IEnumerable<InvoiceDto>> GetInvoices()
        {
            var invoices = service.GetAllInvoices();
            var dtoList = mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            return Ok(dtoList);
        }

        // GET api/invoice/5
        [HttpGet("{InvoiceNumber}")]
        public ActionResult<InvoiceDto> GetInvoice([FromRoute] string invoiceNumber)
        {
            var invoice = service.GetInvoice(invoiceNumber);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<InvoiceDto>(invoice));
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
        public IActionResult CreateInvoice([FromBody] InvoiceForCreationDto invoice)
        {
            try
            {
                Invoice created = service.CreateInvoice(invoice);
                if (created != null)
                {
                    var createdDto = mapper.Map<InvoiceDto>(created);
                    return CreatedAtAction(
                       "GetInvoice",
                       new { created.InvoiceNumber },
                       createdDto);
                }
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(500, "Failed to create new invoice.");
        }

        // PUT: api/invoice/5
        [HttpPut("{InvoiceNumber}")]
        public IActionResult ModifyInvoice([FromRoute] string invoiceNumber,
            [FromBody] InvoiceForUpdateDto invoice)
        {
            try
            {
                if (!service.InvoiceExists(invoiceNumber))
                {
                    return NotFound();
                }

                service.ModifyInvoice(invoiceNumber, invoice);

                return Ok(mapper.Map<InvoiceDto>(service.GetInvoice(invoiceNumber)));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/invoice/5/status
        [HttpPut("{InvoiceNumber}/status")]
        public async Task<IActionResult> IssueInvoice([FromRoute] string invoiceNumber,
            [FromBody] InvoiceStatusDto status)
        {
            InvoiceStatus newStatus = status.Status;
            var invoice = service.GetInvoice(invoiceNumber);

            if (invoice == null)
            {
                return NotFound();
            }

            if (newStatus != InvoiceStatus.Issued || invoice.Status != InvoiceStatus.Draft)
            {
                return BadRequest("The only permitted status change is from 'Draft' to 'Issued'");
            }

            if (invoice.GrandTotal == 0m)
            {
                return BadRequest("The invoice must have a grand total greater than 0.");
            }

            await service.IssueInvoice(invoiceNumber);

            return Ok(mapper.Map<InvoiceDto>(service.GetInvoice(invoiceNumber)));
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
