using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WebApp.Models
{
    public enum InvoiceStatus { New = 0, Draft = 1, Sent = 2, Paid = 3, Cancelled = 4 }

    public class Invoice : IInvoiceHeader
    {
        public Invoice()
        {
            InvoiceLine = new HashSet<InvoiceLine>();
        }

        #region Properties
        [JsonIgnore]
        [BindNever]
        public int Id { get; set; } // the PK is not visible to the client
        public string InvoiceNumber { get; set; } // the alternate key is used instead

        // read/write for the client
        public string ClientName { get; set; }
        public string ClientContactPerson { get; set; }
        public string ClientContact { get; set; }
        public DateTime DateDue { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InvoiceStatus Status { get; set; } // must be New on create

        // read-only for the client; set by the server when invoice is created
        [BindNever]
        public DateTime DateCreated { get; set; }

        [BindNever]
        public string GstNumber { get; set; }

        [BindNever]
        public string CharitiesNumber { get; set; }

        [BindNever]
        public decimal GstRate { get; set; }
        #endregion

        #region Navigation properties and foreign keys
        // Foreign Key commented as EF Core will auto add it to model as shadow property
        // public int StatusId { get; set; } 

        // navigation property
        public ICollection<InvoiceLine> InvoiceLine { get; set; }
        #endregion

        #region Computed fields
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
                return SubTotal + SubTotal * GstRate;
            }
        }
        #endregion
    }
}
