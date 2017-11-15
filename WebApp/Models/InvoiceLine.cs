using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        // Foreign Key(s) (commented because EF Core will add them to the model automatically
        // as shadow properties)
        // public int InvoiceId { get; set; }

        // Navigation property
        // JsonIgnore prevents circular reference when serialising invoices
        [JsonIgnore]
        public Invoice Invoice { get; set; }
    }
}
