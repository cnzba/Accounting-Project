using System.IO;
using System.Threading.Tasks;
using CryptoService;
using Microsoft.AspNetCore.Hosting;
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
        private IHostingEnvironment _env;

        public ChangePasswordController(CBAContext context, ICryptography crypto, IEmailService emailService, IOptions<EmailConfig> emailConfig, IHostingEnvironment env)
        {
            _context = context;
            _crypto = crypto;
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
            _env = env;
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

            var pathToFile = _env.ContentRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "ChangePasswordTemplate.html";

            StreamReader SourceReader = System.IO.File.OpenText(pathToFile);
            string htmlBody = SourceReader.ReadToEnd();
            SourceReader.Close();

            Email emailContent = new Email()
            {
                To = passwordModel.Email,
                Subject = "Password Change",
                Body = string.Format(htmlBody, user.Name)
            };
            await emailService.SendEmail(emailConfig, emailContent);
            return Ok("Password updated successfully");
        }
    }
}
