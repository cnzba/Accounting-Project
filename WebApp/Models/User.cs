using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class User
    {
        [JsonIgnore] [BindNever]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        [JsonIgnore] [BindNever]
        public string Password { get; set; }
        public bool Active { get; set; }

        internal Task ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
