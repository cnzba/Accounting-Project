using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class InvoiceForCreationDto
    {
        [Required(ErrorMessage = "The client's name is required.")]
        public string ClientName { get; set; }
        public string ClientContactPerson { get; set; }

        [StringLength(50)]
        public string PurchaseOrderNumber { get; set; }
        public string ClientContact { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The client's email address is required.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateDue { get; set; }

        public string LoginId { get; set; }
        public ICollection<InvoiceLineDto> InvoiceLine { get; set; }
    }
}
