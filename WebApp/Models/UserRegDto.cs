using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.Models
{
    public class UserRegDto
    {
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string OrgName { get; set; }
        public string OrgCode { get; set; }

        public string StreetAddrL1 { get; set; }
        public string StreetAddrL2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OrgPhoneNumber { get; set; }
        public string LogoURL { get; set; }
        public string CharitiesNumber { get; set; }
        public string GSTNumber { get; set; }

}
}
