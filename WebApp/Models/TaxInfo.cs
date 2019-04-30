using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class TaxInfo
    {
        [JsonIgnore][BindNever]
        [Key]
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public string TaxValue { get; set; }


        //navigation property
        public ICollection<Products> Products { get; set; }

  
    }
}
