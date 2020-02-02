using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IInvoiceService
    {
        Invoice CreateInvoice(InvoiceForCreationDto invoice);
        string GenerateInvoiceNumber();
        IEnumerable<Invoice> GetAllInvoices();

        IEnumerable<Invoice> GetInvoicesByStatus(short invStatus );
        Invoice GetInvoice(string invoiceNumber);
        Invoice GetInvoiceByPaymentId(string paymentId);
        bool InvoiceExists(string invoiceNumber);
        void ModifyInvoice(string invoiceNumber, InvoiceForUpdateDto invoice);
        bool DeleteInvoice(string invoiceNumber);

        IEnumerable<Invoice> GetAllInvoicesBy(string keyword, string sort);

        string GenerateOrganisationInvoiceNumber(string loginId);
        Task IssueInvoice(string invoiceNumber);
        string GetPdfInvoice(string invoiceNumber);

        decimal GetTotalByStatus(short invStatus);
    }
}
