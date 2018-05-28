using System.Collections.Generic;
using WebApp.Models;

namespace WebApp
{
    public interface IInvoiceService
    {
        Invoice CreateInvoice(DraftInvoice invoice);
        string GenerateInvoiceNumber();
        IEnumerable<Invoice> GetAllInvoices();
        IEnumerable<IInvoiceHeader> GetInvoiceHeaders();
        Invoice GetInvoice(string invoiceNumber);
        Invoice GetInvoiceByPaymentId(string paymentId);
        bool InvoiceExists(string invoiceNumber);
        bool ModifyInvoice(DraftInvoice invoice);
        bool DeleteInvoice(string invoiceNumber);
    }
}
