using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string StreetAddressOne { get; set; }
        public string StreetAddressTwo { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public string Phone { get; set; }
        [Required]
        public string ContactFirstName { get; set; }
        [Required]
        public string ContactLastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string InvoiceDeliveryContactFirstName { get; set; }
        public string InvoiceDeliveryContactLastName { get; set; }
        public string InvoiceDeliveryEmail { get; set; }
        
        public string Status { get; set; }
        [Required]
        public string AccountID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Organisation Organisation { get; set; }
    }
}
