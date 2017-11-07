using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public partial class Invoice
    {
        public decimal SubTotal
        {
            get
            {
                return (from il in InvoiceLine select il.Amount).Sum();
            }
        }

        public decimal GrandTotal
        {
            get
            {
                return SubTotal + Gst;
            }
        }
    }
}
