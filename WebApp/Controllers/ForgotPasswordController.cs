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
    public class ForgotPasswordController : Controller
    {
        private readonly ICryptography _crypto;
        private readonly CBAContext _context;
        private readonly IEmailService emailService;
        private readonly IEmailConfig emailConfig;

        public ForgotPasswordController(CBAContext context, ICryptography crypto, IEmailService emailService, IOptions<EmailConfig> emailConfig)
        {
            _context = context;
            _crypto = crypto;
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
        }

        // POST: api/ForgotPassword
        // Body: "email @email.com"
        [HttpPost]
        public async Task<IActionResult> PostForgotPassword([FromBody] EmailModel emailModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid email");
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Email.Equals(emailModel.Email));

            if (user == null)
            {
                return NotFound("Invalid email");
            }

            // Generate temp password, send email, set ForcePasswordChange to true
            string tempPassword = _crypto.GenerateTempPassword(8);

            user.Password = _crypto.HashMD5(tempPassword);
            user.ForcePasswordChange = true;
            await _context.SaveChangesAsync();
            StringBuilder builder = new StringBuilder();

            Email emailContent = new Email()
            {
                To = emailModel.Email,
                Subject = "Request for Password Reset",
                Body = builder.Append("Dear ").Append(user.Name)
                        .Append(",\r\n\r\n\r\n")
                        .Append("Your password has been reset. To login, kindly use the temporary password given below.\r\n\r\n")
                        .Append(tempPassword).Append("\r\n\r\n\r\n")
                        .Append("Kindly note, this password is valid for 15 days only. It is mandatory that you change your password upon login.")
                        .Append("\r\n\r\n\r\n").Append("Best Regards,\r\n\r\n")
                        .Append("CBA New Zealand").ToString()
            };
            if (!await emailService.SendEmail(emailConfig, emailContent))
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
            return Ok("Email has been sent");
        }
    }
}
