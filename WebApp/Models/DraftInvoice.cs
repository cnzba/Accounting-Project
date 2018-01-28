using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApp.Models
{
    public class DraftInvoice
    {
        public DraftInvoice()
        {
            InvoiceLine = new HashSet<InvoiceLine>();
        }

        #region Properties
        [Required]
        public string InvoiceNumber { get; set; } 

        // read/write for the client
        [MinLength(1, ErrorMessage = "ClientName cannot be empty")]
        public string ClientName { get; set; }
        public string ClientContactPerson { get; set; }
        public string ClientContact { get; set; }

        public DateTime DateDue { get; set; }
        #endregion

        #region Navigation properties and foreign keys
        // Foreign Key commented as EF Core will auto add it to model as shadow property
        // public int StatusId { get; set; } 

        // navigation property
        public ICollection<InvoiceLine> InvoiceLine { get; set; }
        #endregion

    }
}
