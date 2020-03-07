using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class CustomerDto
    {
        public string Name { get; set; }
        
        public string StreetAddressOne { get; set; }
        public string StreetAddressTwo { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        public string Phone { get; set; }
        
        public string ContactFirstName { get; set; }
        
        public string ContactLastName { get; set; }
        
        public string Email { get; set; }
        public string InvoiceDeliveryContactFirstName { get; set; }
        public string InvoiceDeliveryContactLastName { get; set; }
        public string InvoiceDeliveryEmail { get; set; }

        public string Status { get; set; }
        
        public string AccountID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int OrganisationId { get; set; }
    }
}
