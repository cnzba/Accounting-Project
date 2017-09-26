using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Customers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
