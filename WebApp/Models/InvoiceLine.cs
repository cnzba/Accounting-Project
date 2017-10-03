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

        public Invoice Invoice { get; set; }
    }
}
