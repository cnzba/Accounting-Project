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
            return context.Invoice.Include("InvoiceLine").OrderByDescending(i => i.DateCreated).ToList();
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
                //InvoiceNumber = GenerateInvoiceNumber(),
                InvoiceNumber = GenerateOrganisationInvoiceNumber(draftInvoice.LoginId),
                CharitiesNumber = options.CharitiesNumber,
                GstNumber = options.GSTNumber,
                GstRate = options.GSTRate,
                DateCreated = localNow,
                Status = InvoiceStatus.Draft,
                Creator = context.User.FirstOrDefault(u => u.Email == draftInvoice.LoginId)
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

        public string GenerateOrganisationInvoiceNumber(string loginId)
        {
            
            var user = context.User.Include(u => u.Organisation).FirstOrDefault(u => u.Email == loginId);
            if (user == null && user.Organisation == null)
                return null;

            //var org = context.Organisation.Find(user.Organisation.Id);

            var lastInvoice = context.Invoice.Where(i => i.Creator.Organisation.Id == user.Organisation.Id)
                .OrderByDescending(i=>i.InvoiceNumber).First();
            if(lastInvoice == null)
            {
                return user.Organisation.Code + "000001";
            }

            string invoiceSequentialNumber = lastInvoice.InvoiceNumber.Substring(3);
            try
            {
                int sn = int.Parse(invoiceSequentialNumber);
                return user.Organisation.Code + (sn + 1).ToString("D6");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            return user.Organisation.Code + "000000";
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

            if(!string.IsNullOrEmpty(keyword))
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
    }
}
