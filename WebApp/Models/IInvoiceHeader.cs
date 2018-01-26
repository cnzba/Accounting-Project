using System;

namespace WebApp.Models
{
    public interface IInvoiceHeader
    {
        string CharitiesNumber { get; set; }
        string ClientContact { get; set; }
        string ClientContactPerson { get; set; }
        string ClientName { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateDue { get; set; }
        decimal GrandTotal { get; }
        string GstNumber { get; set; }
        decimal GstRate { get; set; }
        string InvoiceNumber { get; set; }
        InvoiceStatus Status { get; set; }
        decimal SubTotal { get; }
    }
}