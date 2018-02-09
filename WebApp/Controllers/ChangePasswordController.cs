using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiceUtil.Email;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ChangePasswordController : Controller
    {
        private readonly ICryptography _crypto;
        private readonly CBAContext _context;
        private readonly IEmailService emailService;
        private readonly IEmailConfig emailConfig;

        public ChangePasswordController(CBAContext context, ICryptography crypto, IEmailService emailService, IOptions<EmailConfig> emailConfig)
        {
            _context = context;
            _crypto = crypto;
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
        }

        // POST: api/ChangePassword
        // Body: { "email" : "email@email.com", "old_password" : "oldPassword", "newPassword" : "new_password" }
        [HttpPost]
        public async Task<IActionResult> PostChangePassword([FromBody] PasswordModel passwordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Email.Equals(passwordModel.Email));

            if (user == null)
            {
                return NotFound("Invalid user");
            }

            if (_crypto.HashMD5(passwordModel.OldPassword) != user.Password)
            {
                return BadRequest("Old password is incorrect");
            }

            user.Password = _crypto.HashMD5(passwordModel.NewPassword);

            if (user.ForcePasswordChange)
            {
                user.ForcePasswordChange = false;
            }

            await _context.SaveChangesAsync();
            StringBuilder builder = new StringBuilder();

            Email emailContent = new Email()
            {
                To = passwordModel.Email,
                Subject = "Password Change",
                Body = builder.Append("Dear ").Append(user.Name).Append(",\r\n\r\n\r\n")
                        .Append("Your password has been changed successfully. If you did not change your password, kindly contact admin@cbanewzealand.org.nz immediately.")
                        .Append("\r\n\r\n\r\n").Append("Best Regards,\r\n\r\n")
                        .Append("CBA New Zealand").ToString()
            };
            await emailService.SendEmail(emailConfig, emailContent);
            return Ok("Password updated successfully");
        }
    }
}
