using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace WebApp.Models
{
    public class Products
    {
        [JsonIgnore]
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public int TaxId { get; set; }


        [JsonIgnore]
        public TaxInfo TaxInfo { get; set; }

        //navigation property
        public ICollection<InvoiceLine> InvoiceLine { get; set; }
    }
}
