using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class VerifyTokenDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
