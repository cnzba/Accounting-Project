using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ResetPasswordDto: VerifyTokenDto
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
