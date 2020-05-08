using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CryptoService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IEmailService emailService;
        private readonly IEmailConfig emailConfig;
        private UserManager<CBAUser> _userManager;
        public ForgotPasswordController(IEmailService emailService,
            IOptions<EmailConfig> emailConfig, UserManager<CBAUser> userManager)
        {           
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
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

            var cbaUser = await _userManager.FindByEmailAsync(emailModel.Email);

            if (cbaUser == null)
            {
                return BadRequest("User does not exist..!!");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(cbaUser);

            var hostAddress = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            //Multiple Parameters
            var queryParams = new Dictionary<string, string>()
            {
                {"id", cbaUser.Id+"" },
                {"token", token+"" }
            };

            string passwordResetLink = QueryHelpers.AddQueryString($"{hostAddress}/reset-password", queryParams);

            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(),
                             "EmailTemplates",
                             "ForgotPasswordTemplate.html");

            StreamReader SourceReader = System.IO.File.OpenText(pathToFile);
            string htmlBody = SourceReader.ReadToEnd();
            SourceReader.Close();

            Email emailContent = new Email()
            {
                To = emailModel.Email,
                Subject = "Request for Password Reset",
                Body = string.Format(htmlBody, cbaUser.FirstName, passwordResetLink)
            };
            if (!await emailService.SendEmail(emailConfig, emailContent))
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
                //return BadRequest("Error has been occured during sending mail. Please try again after some time..!!");
            }
            return Ok("An email has been sent with instruction to reset your password...!!");
        }
    }
}
