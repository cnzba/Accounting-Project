using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceUtil.Email;
using WebApp.Entities;
using System.IO;
using WebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResetPasswordController : Controller
    {
        private readonly IEmailService emailService;
        private readonly IEmailConfig emailConfig;
        private UserManager<CBAUser> _userManager;

        public ResetPasswordController(IEmailService emailService,
            IOptions<EmailConfig> emailConfig, UserManager<CBAUser> userManager)
        {
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostVerifyToken([FromBody]VerifyTokenDto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var cbaUser = await _userManager.FindByIdAsync(obj.Id);
            if (cbaUser == null)
                return BadRequest("Invalid User..!!");

            var result = await _userManager.VerifyUserTokenAsync(cbaUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", obj.Token);

            if (!result)
            {
                return BadRequest("Invalid token..!!");
            }
            return Ok();
        }


        [HttpPost]
        [Route("PostChangePassword")]
        public async Task<IActionResult> PostChangePassword([FromBody] ResetPasswordDto passwordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            var cbaUser = await _userManager.FindByIdAsync(passwordModel.Id);

            if (cbaUser == null)
            {
                return NotFound("Invalid user");
            }

            var result = await _userManager.ResetPasswordAsync(cbaUser, passwordModel.Token, passwordModel.NewPassword);

            if (result.Succeeded)
            {
                var pathToFile = Directory.GetCurrentDirectory()
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "ChangePasswordTemplate.html";

                StreamReader SourceReader = System.IO.File.OpenText(pathToFile);
                string htmlBody = SourceReader.ReadToEnd();
                SourceReader.Close();

                Email emailContent = new Email()
                {
                    To = cbaUser.Email,
                    Subject = "Reset Password",
                    Body = string.Format(htmlBody, cbaUser.FirstName)
                };
                if (!await emailService.SendEmail(emailConfig, emailContent))
                {
                    return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
                }
                return Ok();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.Description);
                }
                return BadRequest(sb.ToString());
            }
        }
    }
}
