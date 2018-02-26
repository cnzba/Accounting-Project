using System.Collections.Generic;
using WebApp.Models;

namespace WebApp
{
    public interface IInvoiceService
    {
        bool CreateInvoice(DraftInvoice invoice);
        string GenerateInvoiceNumber();
        IEnumerable<Invoice> GetAllInvoices();
        IEnumerable<IInvoiceHeader> GetInvoiceHeaders();
        Invoice GetInvoice(string invoiceNumber);
        bool InvoiceExists(string invoiceNumber);
        bool ModifyInvoice(DraftInvoice invoice);
        bool DeleteInvoice(int Id);
    }
}
