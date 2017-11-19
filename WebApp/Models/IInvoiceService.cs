using System.Collections.Generic;
using WebApp.Models;

namespace WebApp
{
    public interface IInvoiceService
    {
        bool CreateInvoice(Invoice invoice);
        string GenerateInvoiceNumber();
        IEnumerable<Invoice> GetAllInvoices();
        Invoice GetInvoice(string invoiceNumber);
        bool InvoiceExists(string invoiceNumber);
        bool ModifyInvoice(Invoice invoice);
    }
}
