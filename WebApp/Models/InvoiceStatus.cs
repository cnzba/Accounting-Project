using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class InvoiceStatus
    {
        public InvoiceStatus() {}

        public int Id { get; set; }
        public string Status { get; set; }
    }
}
