using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class InvoiceLine
    {
        [JsonIgnore]
        public int Id { get; set; } // the PK is not needed by the client
        public int ItemOrder { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        // Foreign Key(s) (commented because EF Core will add them to the model automatically
        // as shadow properties)
        // public int InvoiceId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Invoice Invoice { get; set; }
    }
}
