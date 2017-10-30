using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Options
{
    public class CBAOptions
    {
        // public parameterless constructor required in order to act as a service for DI
        public CBAOptions() { }

        public decimal GSTRate { get; set; }
        public string GSTNumber { get; set; }
        public string CharitiesNumber { get; set; }
    }
}
