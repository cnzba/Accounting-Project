using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceUtil.Email;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Email")]
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IEmailService emailService;
        private readonly IEmailConfig emailConfig;
        private readonly IEmail email;

        public EmailController(IEmailService emailService, IEmail email, IOptions<EmailConfig> emailConfig)       
        {
            this.emailService = emailService;
            this.emailConfig = emailConfig.Value;           
            this.email = email;
        }

        //Todo        
        // Change Parameter Type Email to IEmail 
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody]Email EmailSend)
        {
            if (EmailSend == null)
            {
                throw new System.ArgumentNullException(nameof(EmailSend));
            }

            var ret = await emailService.SendEmail(emailConfig, EmailSend);
            if (ret)
                return Ok(ret);
            else
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }


    }
}
