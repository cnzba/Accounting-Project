using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ResetPasswordModel: VerifyTokenModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
