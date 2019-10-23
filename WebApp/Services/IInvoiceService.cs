using System.Collections.Generic;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IInvoiceService
    {
        Invoice CreateInvoice(InvoiceForCreationDto invoice);
        string GenerateInvoiceNumber();
        IEnumerable<Invoice> GetAllInvoices();
        Invoice GetInvoice(string invoiceNumber);
        Invoice GetInvoiceByPaymentId(string paymentId);
        bool InvoiceExists(string invoiceNumber);
        void ModifyInvoice(string invoiceNumber, InvoiceForUpdateDto invoice);
        bool DeleteInvoice(string invoiceNumber);

        IEnumerable<Invoice> GetAllInvoicesBy(string keyword, string sort);

        string GenerateOrganisationInvoiceNumber(string loginId);
        void IssueInvoice(string invoiceNumber);
    }
}
