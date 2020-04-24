using System.IO;
using System.Threading.Tasks;
using CryptoService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiceUtil.Email;
using WebApp.Entities;

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
        private IHostingEnvironment _env;
        private UserManager<CBAUser> _userManager;
        public ForgotPasswordController(CBAContext context, ICryptography crypto, IEmailService emailService,
            IOptions<EmailConfig> emailConfig, IHostingEnvironment env, UserManager<CBAUser> userManager)
        {
            _context = context;
            _crypto = crypto;
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
            _env = env;
            _userManager = userManager;
        }

        // POST: api/ForgotPassword
        // Body: { "email" : "email@email.com" }
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

            CBAUser objCBAUser = new CBAUser()
            {
                Email = user.Email,             
            };

            string tempPassword = await _userManager.GeneratePasswordResetTokenAsync(objCBAUser);
            // Generate temp password, send email, set ForcePasswordChange to true
            //string tempPassword = _crypto.GenerateTempPassword(8);

            //user.Password = _crypto.HashMD5(tempPassword);
            //user.ForcePasswordChange = true;
            //await _context.SaveChangesAsync();

            var pathToFile = _env.ContentRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "ForgotPasswordTemplate.html";

            StreamReader SourceReader = System.IO.File.OpenText(pathToFile);
            string htmlBody = SourceReader.ReadToEnd();
            SourceReader.Close();

            Email emailContent = new Email()
            {
                To = emailModel.Email,
                Subject = "Request for Password Reset",
                Body = string.Format(htmlBody, user.Name, tempPassword)
            };
            if (!await emailService.SendEmail(emailConfig, emailContent))
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
            return Ok("Email has been sent");
        }
    }
}
