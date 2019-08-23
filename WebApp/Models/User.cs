using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class User
    {
        [JsonIgnore] [BindNever]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        [JsonIgnore] [BindNever]
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool ForcePasswordChange { get; set; }

        internal Task ToListAsync()
        {
            throw new NotImplementedException();
        }

        public Organisation Organisation { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
