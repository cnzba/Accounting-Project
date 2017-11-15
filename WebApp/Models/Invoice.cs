using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models
{
    public enum InvoiceStatus { New = 0, Sent = 1, Paid = 2 }

    public class Invoice
    {
        public Invoice()
        {
            InvoiceLine = new HashSet<InvoiceLine>();
        }

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

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateDue { get; set; }

        public string InvoiceNumber { get; set; }
        public string IssueeOrganization { get; set; }
        public string IssueeCareOf { get; set; }
        public string ClientContact { get; set; }

        public string GstNumber { get; set; }
        public string CharitiesNumber { get; set; }

        public decimal Gst { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InvoiceStatus Status { get; set; }

        // Foreign Keys (commented because EF Core will add them to the model automatically
        // as shadow properties)
        // public int StatusId { get; set; }

        // navigation property
        public ICollection<InvoiceLine> InvoiceLine { get; set; }
    }
}
