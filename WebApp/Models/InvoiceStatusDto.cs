using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.Models
{
    public class InvoiceStatusDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public InvoiceStatus Status { get; set; }
    }
}
