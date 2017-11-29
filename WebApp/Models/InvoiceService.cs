using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return context.Invoice.Include("InvoiceLine").OrderBy(i => i.DateCreated).ToList();
        }

        public Invoice GetInvoice(string invoiceNumber)
        {
            var invoice = context.Invoice.Include("InvoiceLine")
                .SingleOrDefault(t => t.InvoiceNumber == invoiceNumber);

            return invoice;
        }

        public bool CreateInvoice(Invoice invoice)
        {
            invoice.InvoiceNumber = GenerateInvoiceNumber();
            invoice.CharitiesNumber = options.CharitiesNumber;
            invoice.GstNumber = options.GSTNumber;
            invoice.GstRate = options.GSTRate;
            invoice.DateCreated = DateTime.Now;
            invoice.Status = InvoiceStatus.New;
            
            context.Add<Invoice>(invoice);
            return context.SaveChanges() > 0;
        }

        public string GenerateInvoiceNumber()
        {
            return string.Format("CBA{0:N}", Guid.NewGuid());
        }

        public bool ModifyInvoice(Invoice invoice)
        {
            var invoiceToUpdate = context.Invoice.SingleOrDefault(n => n.InvoiceNumber == invoice.InvoiceNumber);

            invoiceToUpdate.IssueeCareOf = invoice.IssueeCareOf;
            invoiceToUpdate.IssueeOrganization = invoice.IssueeOrganization;
            invoiceToUpdate.ClientContact = invoice.ClientContact;
            invoiceToUpdate.DateDue = invoice.DateDue;
            invoiceToUpdate.Status = invoice.Status;

            // every time an invoice is modified, its item lines are completely rewritten
            invoiceToUpdate.InvoiceLine.Clear();
            invoiceToUpdate.InvoiceLine = invoice.InvoiceLine;

            if (!context.Entry(invoiceToUpdate).State.HasFlag(EntityState.Modified)) return true;
            else return context.SaveChanges() > 0;
        }

        public bool InvoiceExists(string invoiceNumber)
        {
            return context.Invoice.Any(e => e.InvoiceNumber == invoiceNumber);
        }
    }
}
