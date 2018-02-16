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

        private void validate(Invoice invoice)
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

        public bool CreateInvoice(DraftInvoice draftInvoice)
        {
            Invoice invoice = new Invoice();

            // copy the information supplied by the client
            invoice.ClientName = draftInvoice.ClientName;
            invoice.ClientContactPerson = draftInvoice.ClientContactPerson;
            invoice.ClientContact = draftInvoice.ClientContact;
            invoice.DateDue = draftInvoice.DateDue;
            invoice.InvoiceLine = draftInvoice.InvoiceLine;

            // add the server generated information
            invoice.InvoiceNumber = GenerateInvoiceNumber();
            invoice.CharitiesNumber = options.CharitiesNumber;
            invoice.GstNumber = options.GSTNumber;
            invoice.GstRate = options.GSTRate;
            invoice.DateCreated = DateTime.Now;
            invoice.Status = InvoiceStatus.Draft;

            validate(invoice);

            context.Add<Invoice>(invoice);
            int count = context.SaveChanges();
            return count > 0;
        }

        public string GenerateInvoiceNumber()
        {
            return string.Format("CBA{0:N}", Guid.NewGuid());
        }

        public bool ModifyInvoice(DraftInvoice invoice)
        {
            var invoiceToUpdate = GetInvoice(invoice.InvoiceNumber);

            if(invoiceToUpdate.Status != InvoiceStatus.Draft) throw new ArgumentException($"Invoice {invoice.InvoiceNumber} is not a draft and may not be modified");

            invoiceToUpdate.ClientContactPerson = invoice.ClientContactPerson;
            invoiceToUpdate.ClientName = invoice.ClientName;
            invoiceToUpdate.ClientContact = invoice.ClientContact;
            invoiceToUpdate.DateDue = invoice.DateDue;

            // every time an invoice is modified, it's item lines are completely rewritten
            invoiceToUpdate.InvoiceLine.Clear();
            invoiceToUpdate.InvoiceLine = invoice.InvoiceLine;

            validate(invoiceToUpdate);

            if (!context.ChangeTracker.HasChanges()) return true;
            else return context.SaveChanges() > 0;
        }

        public bool InvoiceExists(string invoiceNumber)
        {
            return context.Invoice.Any(e => e.InvoiceNumber == invoiceNumber);
        }
    }
}
