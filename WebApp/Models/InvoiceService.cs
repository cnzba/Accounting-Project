using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApp.Models;
using WebApp.Options;

namespace WebApp
{
    public class InvoiceService : IInvoiceService
    {
        private CBAContext context;
        private CBAOptions options;

        public InvoiceService(CBAContext context, IOptions<CBAOptions> optionsAccessor)
        {
            this.context = context;
            options = optionsAccessor.Value;
        }

        private void Validate(Invoice invoice)
        {
            Validator.ValidateObject(invoice, new ValidationContext(invoice), true);
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return context.Invoice.Include("InvoiceLine").OrderBy(i => i.DateCreated).ToList();
        }

        public IEnumerable<IInvoiceHeader> GetInvoiceHeaders()
        {
            return context.Invoice.Include("InvoiceLine").OrderBy(i => i.DateCreated).ToList();
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

        public Invoice CreateInvoice(DraftInvoice draftInvoice)
        {
            // determine current New Zealand time
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            Invoice invoice = new Invoice
            {

                // copy the information supplied by the client
                ClientName = draftInvoice.ClientName,
                ClientContactPerson = draftInvoice.ClientContactPerson,
                ClientContact = draftInvoice.ClientContact,
                Email = draftInvoice.Email,
                DateDue = draftInvoice.DateDue,
                InvoiceLine = draftInvoice.InvoiceLine,

                // add the server generated information
                InvoiceNumber = GenerateInvoiceNumber(),
                CharitiesNumber = options.CharitiesNumber,
                GstNumber = options.GSTNumber,
                GstRate = options.GSTRate,
                DateCreated = localNow,
                Status = InvoiceStatus.Draft
            };

            Validate(invoice);

            context.Add<Invoice>(invoice);
            int count = context.SaveChanges();

            if (count > 0) return invoice; else return null;
        }

        public string GenerateInvoiceNumber()
        {
            return string.Format("CBA{0:N}", Guid.NewGuid());
        }

        public bool ModifyInvoice(DraftInvoice invoice)
        {
            var invoiceToUpdate = GetInvoice(invoice.InvoiceNumber);

            if (invoiceToUpdate.Status != InvoiceStatus.Draft) throw new ArgumentException($"Invoice {invoice.InvoiceNumber} is not a draft and may not be modified");

            invoiceToUpdate.ClientContactPerson = invoice.ClientContactPerson;
            invoiceToUpdate.ClientName = invoice.ClientName;
            invoiceToUpdate.ClientContact = invoice.ClientContact;
            invoiceToUpdate.DateDue = invoice.DateDue;

            // every time an invoice is modified, its item lines are completely rewritten
            invoiceToUpdate.InvoiceLine.Clear();
            invoiceToUpdate.InvoiceLine = invoice.InvoiceLine;

            Validate(invoiceToUpdate);

            if (!context.ChangeTracker.HasChanges()) return true;
            else return context.SaveChanges() > 0;
        }

        public bool InvoiceExists(string invoiceNumber)
        {
            return context.Invoice.Any(e => e.InvoiceNumber == invoiceNumber);
        }

        public bool DeleteInvoice(string invoiceNumber)
        {
            var invoice = context.Invoice.SingleOrDefault(n => n.InvoiceNumber == invoiceNumber);

            if (invoice == null) throw new ArgumentOutOfRangeException();
            if(invoice.Status != InvoiceStatus.Draft) throw new ArgumentException();

            context.Remove<Invoice>(invoice);
            return context.SaveChanges() > 0;
        }
    }
}
