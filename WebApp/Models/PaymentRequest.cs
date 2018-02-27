using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PaymentRequest
    {
        [Required]
        public string TokenId { get; set; }

        [Required]
        public string TokenObj { get; set; }

        [Required]
        public string InvoiceNo { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Gateway { get; set; }
    }
}
