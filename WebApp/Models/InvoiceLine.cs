using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class InvoiceLine
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        // JsonIgnore prevents circular reference when serialising invoices
        [JsonIgnore]
        public Invoice Invoice { get; set; }
    }
}
