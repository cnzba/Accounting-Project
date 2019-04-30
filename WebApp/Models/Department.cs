using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Department
    {
        [JsonIgnore]
        [BindNever]
        public int DepartmentId { get; set; }
        public string DepartmentAddress { get; set; }
        public bool Deleted { get; set; }
       

        internal Task ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
