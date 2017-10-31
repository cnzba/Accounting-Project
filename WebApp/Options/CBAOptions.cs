using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Options
{
    /// <summary>
    /// This class binds to the configuration values in the database.
    /// It is currently read only. Modified properties won't be reflected in the database.
    /// </summary>
    public class CBAOptions
    {
        // public parameterless constructor required in order to act as a service for DI
        public CBAOptions() { }

        public decimal GSTRate { get; set; }
        public string GSTNumber { get; set; }
        public string CharitiesNumber { get; set; }
    }
}
