using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceUtil.Email;
using WebApp.Controllers;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestEmail
    {

        [TestMethod]
        public async System.Threading.Tasks.Task SendEmail_TestAsync()
        {
            IEmailService service = new EmailService();
            var email = new Email();


            //Mock AppSetings.json
            EmailConfig EmailConfig = new EmailConfig()
            {
                FromAddress = "testuser@cbanewzealand.org.nz",
                FromName = "Unit Test Email",
                LocalDomain = "cbanewzealand.onmicrosoft.com",
                MailServerAddress = "smtp.office365.com",
                MailServerPort = 587,
                UserId = "testuser@cbanewzealand.org.nz",
                UserPassword = "123@change"
            };
            IOptions<EmailConfig> MockAppSettings = Options.Create(EmailConfig);

            // Web API Controller
            EmailController emailController = new EmailController(service, email, MockAppSettings);

            //Email teste
            email.Subject = "Account Software email sent - Delete me";
            email.To = "helersonlage@gmail.com";
            email.Body = "Hi There, <br/><p>This is a test email from <b>Account<b/> software</p><br/><p> Kind regards <br/> Canterbury & New Zealand Business Association </p>";
            email.Cc.Add("helersonlage@gmail.com");
            email.Bco.Add("helersonlage@gmail.com");
           // email.Attachment.Add(@"c:\Test\TestAttachment.png");            

           var codeReturn = await emailController.SendEmail(email);
          
            Assert.IsTrue(codeReturn is OkObjectResult);
        }

    }
}
