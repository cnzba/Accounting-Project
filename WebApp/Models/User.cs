using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        internal Task ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
