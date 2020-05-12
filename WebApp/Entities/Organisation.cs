using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    public class Organisation
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string StreetAddressOne { get; set; }
        public string StreetAddressTwo { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }
        public string CharitiesNumber { get; set; }
        public string GSTNumber { get; set; }
                
        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }

        public virtual ICollection<CBAUser> Users { get; set; }
    }
}
