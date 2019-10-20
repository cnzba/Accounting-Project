using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class InvoiceLineDto
    {
        public int ItemOrder { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
