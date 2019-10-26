using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    /// <summary>
    /// Represent an Invoice as returned to a client of the api.
    /// </summary>
    public class InvoiceDto
    {
        public string InvoiceNumber { get; set; } 

        public string ClientName { get; set; }
        public string ClientContactPerson { get; set; }

        public string PurchaseOrderNumber { get; set; }
        public string ClientContact { get; set; }
        public string Email { get; set; }

        public DateTime DateDue { get; set; }
        public DateTime DateCreated { get; set; }

        public string PaymentId { get; set; }

        public string Status { get; set; } 

        public decimal GstRate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public ICollection<InvoiceLineDto> InvoiceLine { get; set; }

    }
}
