using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceUtil.Email;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestEmail
    {
        [TestMethod]
        public async System.Threading.Tasks.Task SendEmail_TestAsync()
        {
            IEmailService sendEmail = new EmailService();
            IEmail email = new Email();
          
            email.EmailConfig.FromAddress = "testuser@cbanewzealand.org.nz";
            email.EmailConfig.FromName = "Invoice - Canterbury & New Zealand Business Association";
            email.EmailConfig.LocalDomain = "cbanewzealand.onmicrosoft.com";
            email.EmailConfig.MailServerAddress = "smtp.office365.com";
            email.EmailConfig.MailServerPort = 587;
            email.EmailConfig.UserId = "testuser@cbanewzealand.org.nz";
            email.EmailConfig.UserPassword = "123@change";
            email.Subject = "Account Software email sent";
            email.To = "helersonlage@gmail.com";
           // email.Cc.Add("kgelsey@gmail.com"); 
           // email.Cc.Add("taz@cbanewzealand.org.nz"); 
            email.Body = "Hi There, <br/><p>This is a test email from <b>Account<b/> software</p><br/><p> Kind regards <br/> Canterbury & New Zealand Business Association </p>";
            email.Attachment.Add(@"c:\Test\TestAttachment.png");
           



            Assert.AreEqual(await sendEmail.SendEmail(email), true);
        }

    }
}
