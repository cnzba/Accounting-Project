using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    public class CBAUser:IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public bool IsActive { get; set; }

        //public int OrganisationID { get; set; }

        public virtual Organisation Organisation { get; set; }
    }
}
