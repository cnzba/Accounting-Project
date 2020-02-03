using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Options;

namespace WebApp.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly CBAContext context;
        private readonly CBAOptions options;
        private readonly IMapper mapper;
        private readonly IPdfService pdfService;
        private readonly ILogger<InvoiceService> logger;

        public InvoiceService(
            CBAContext context,
            IOptions<CBAOptions> optionsAccessor,
            IMapper mapper,
            IPdfService pdfService,
            ILogger<InvoiceService> logger)
        {
            this.context = context;
            options = optionsAccessor.Value;
            this.mapper = mapper;
            this.pdfService = pdfService;
            this.logger = logger;
        }

        private void Validate(Invoice invoice)
        {
            Validator.ValidateObject(invoice, new System.ComponentModel.DataAnnotations.ValidationContext(invoice), true);
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return context.Invoice.Include("InvoiceLine").OrderByDescending(i => i.DateCreated).ToList();
        }

        public IEnumerable<Invoice> GetInvoicesByStatus(InvoiceStatus invStatus){
            if (invStatus == InvoiceStatus.Overdue){
                return context.Invoice.Include("InvoiceLine")
                    .Where(inv => inv.DateDue <= DateTime.Today 
                                && inv.Status != InvoiceStatus.Paid
                                && inv.DateCreated.Year.ToString()== DateTime.Now.Year.ToString());
            }
            return context.Invoice.Include("InvoiceLine")
                .Where(inv => inv.Status==invStatus
                            && inv.DateCreated.Year.ToString()== DateTime.Now.Year.ToString());
        }

        public IEnumerable<decimal> GetTotalByStatus(InvoiceStatus invStatus){
            var invs = context.Invoice.Include("InvoiceLine")
                    .Where(inv => inv.Status == invStatus
                                && inv.DateCreated.Year.ToString()== DateTime.Now.Year.ToString());
            var grandTotals = invs.Select(inv => inv.GrandTotal);
            
            return grandTotals;
        }

        public Invoice GetInvoice(string invoiceNumber)
        {
            var invoice = context.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.InvoiceNumber == invoiceNumber);

            return invoice;
        }

        public Invoice GetInvoiceByPaymentId(string paymentId)
        {
            var invoice = context.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.PaymentId == paymentId);

            return invoice;
        }

        public Invoice CreateInvoice(InvoiceForCreationDto inputInvoice)
        {
            // determine current New Zealand time
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            // copy information provided by the client
            Invoice invoice = mapper.Map<Invoice>(inputInvoice);

            invoice.InvoiceNumber = GenerateOrganisationInvoiceNumber(inputInvoice.LoginId);
            invoice.CharitiesNumber = options.CharitiesNumber;
            invoice.GstNumber = options.GSTNumber;
            invoice.GstRate = options.GSTRate;
            invoice.DateCreated = localNow;
            invoice.Status = InvoiceStatus.Draft;
            invoice.Creator = context.User.FirstOrDefault(u => u.Email == inputInvoice.LoginId);

            Validate(invoice);

            context.Add<Invoice>(invoice);
            int count = context.SaveChanges();

            if (count > 0)
            {
                return invoice;
            }
            else
            {
                return null;
            }
        }

        public string GenerateInvoiceNumber()
        {
            return string.Format("CBA{0:N}", Guid.NewGuid());
        }

        public string GenerateOrganisationInvoiceNumber(string loginId)
        {

            var user = context.User.Include(u => u.Organisation).FirstOrDefault(u => u.Email == loginId);
            if (user == null || user.Organisation == null)
            {
                return null;
            }

            //var org = context.Organisation.Find(user.Organisation.Id);

            var lastInvoice = context.Invoice
                .Where(i => i.Creator.Organisation.Id == user.Organisation.Id && i.InvoiceNumber.StartsWith(user.Organisation.Code))
                .OrderByDescending(i => i.InvoiceNumber).FirstOrDefault();
            if (lastInvoice == null)
            {
                return user.Organisation.Code + "000001";
            }

            string invoiceSequentialNumber = lastInvoice.InvoiceNumber.Substring(user.Organisation.Code.Length);    //org code is 4 characters
            try
            {
                int sn = int.Parse(invoiceSequentialNumber);
                return user.Organisation.Code + (sn + 1).ToString("D6");
            }
            catch (Exception ex) when (
                ex is ArgumentNullException ||
                ex is FormatException ||
                ex is OverflowException)
            {
                logger.LogWarning(ex, "Unable to generate the next invoice number in sequence after {0}", invoiceSequentialNumber);
            }

            return null;
        }

        public void ModifyInvoice(string invoiceNumber, InvoiceForUpdateDto invoice)
        {
            var invoiceToUpdate = GetInvoice(invoiceNumber);

            if (invoiceToUpdate.Status != InvoiceStatus.Draft)
            {
                throw new InvalidOperationException($"Invoice {invoiceNumber} is not a draft and may not be modified");
            }

            // apply invoice to invoiceToUpdate
            mapper.Map(invoice, invoiceToUpdate);

            Validate(invoiceToUpdate);

            context.SaveChanges();
        }

        public bool InvoiceExists(string invoiceNumber)
        {
            return context.Invoice.Any(e => e.InvoiceNumber == invoiceNumber);
        }

        public bool DeleteInvoice(string invoiceNumber)
        {
            var invoice = context.Invoice.SingleOrDefault(n => n.InvoiceNumber == invoiceNumber);

            if (invoice == null) throw new ArgumentOutOfRangeException();
            if (invoice.Status != InvoiceStatus.Draft) throw new ArgumentException();

            context.Remove<Invoice>(invoice);
            return context.SaveChanges() > 0;
        }

        public IEnumerable<Invoice> GetAllInvoicesBy(string keyword, string sort)
        {

            IEnumerable<Invoice> result;

            if (string.IsNullOrEmpty(sort))
            {
                sort = "created_asc";
            }

            if (sort.Equals("created_desc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderByDescending(i => i.DateCreated).ToList();
            }
            else if (sort.Equals("due_asc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderBy(i => i.DateDue).ToList();
            }
            else if (sort.Equals("due_desc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderByDescending(i => i.DateDue).ToList();
            }
            else if (sort.Equals("name_asc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderBy(i => i.ClientName).ToList();
            }
            else if (sort.Equals("name_desc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderByDescending(i => i.ClientName).ToList();
            }
            else if (sort.Equals("amount_asc"))
            {
                result = context.Invoice.Include("InvoiceLine").ToList();
                result = result.OrderBy(i => i.GrandTotal);
            }
            else if (sort.Equals("amount_desc"))
            {
                result = context.Invoice.Include("InvoiceLine").ToList();
                result = result.OrderByDescending(i => i.GrandTotal);
            }
            else if (sort.Equals("status_asc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderBy(i => i.Status).ToList();
            }
            else if (sort.Equals("status_desc"))
            {
                result = context.Invoice.Include("InvoiceLine").OrderByDescending(i => i.Status).ToList();
            }
            else // sort=created_asc
            {
                result = context.Invoice.Include("InvoiceLine").OrderBy(i => i.DateCreated).ToList();
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(r => r.ClientName.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || r.DateCreated.ToString("dd'/'MM'/'yyyy").Contains(keyword)
                                        || r.DateDue.ToString("dd'/'MM'/'yyyy").Contains(keyword)
                                        || r.GrandTotal.ToString().Equals(keyword)
                                        || r.Status.ToString().Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                        || r.InvoiceNumber.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return result;
        }

        /// <summary>
        /// Finalise an invoice and email it to the recipient
        /// </summary>
        public async Task IssueInvoice(string invoiceNumber)
        {
            var invoiceToUpdate = GetInvoice(invoiceNumber);
            if (invoiceToUpdate == null) throw new ArgumentOutOfRangeException();
            if (invoiceToUpdate.Status != InvoiceStatus.Draft) throw new InvalidOperationException();
            if (invoiceToUpdate.GrandTotal == 0m) throw new ArgumentException("The invoice must have a grand total of greater than zero.");

            invoiceToUpdate.Status = InvoiceStatus.Issued;

            try
            {
                await pdfService.EmailPdf(invoiceToUpdate);
            }
            catch
            {
                invoiceToUpdate.Status = InvoiceStatus.Draft;
                throw;
            }

            context.SaveChanges();
        }

        public string GetPdfInvoice(string invoiceNumber)
        {
            return pdfService.GetPdfInvoice(invoiceNumber);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
