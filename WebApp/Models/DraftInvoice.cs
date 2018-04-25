using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [Required(ErrorMessage = "The client's name is required.")]
        public string ClientName { get; set; }
        public string ClientContactPerson { get; set; }
        public string ClientContact { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "The client's email address is required.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [NotPast(ErrorMessage = "The due date cannot be in the past.")]
        public DateTime DateDue { get; set; }
        #endregion

        #region Navigation properties and foreign keys
        // Foreign Key commented as EF Core will auto add it to model as shadow property
        // public int StatusId { get; set; } 

        // navigation property
        public ICollection<InvoiceLine> InvoiceLine { get; set; }
        #endregion

        #region Custom validation
        public class NotPast : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime d = Convert.ToDateTime(value);
                return d >= DateTime.Now; 
            }
        }
        #endregion
    }
}
