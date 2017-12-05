using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ServiceUtil.Email
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(IEmailConfig emailConfig, IEmail email)
        {
            try
            {
                MimeMessage emailMessage = CreateEmailBody(email, emailConfig);

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = emailConfig.LocalDomain;
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(emailConfig.MailServerAddress, Convert.ToInt32(emailConfig.MailServerPort), SecureSocketOptions.StartTls).ConfigureAwait(false);

                    await client.AuthenticateAsync(new NetworkCredential(emailConfig.UserId, emailConfig.UserPassword));
                   await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private MimeMessage CreateEmailBody(IEmail email, IEmailConfig emailConfig)
        {
            var emailMessage = new MimeMessage();
            var builder = new BodyBuilder();

            emailMessage.From.Add(new MailboxAddress(emailConfig.FromName, address: emailConfig.FromAddress));
            emailMessage.To.Add(new MailboxAddress(email.To ?? string.Empty));
            emailMessage.Subject = email.Subject ?? string.Empty;

            foreach (var Cc in email.Cc)
            {
                emailMessage.Cc.Add(new MailboxAddress(Cc));
            }
            foreach (var Bbo in email.Bco)
            {
                emailMessage.Bcc.Add(new MailboxAddress(Bbo));
            }
            foreach (var attach in email.Attachment)
            {
                builder.Attachments.Add(attach);
            }
                        
            builder.HtmlBody = email.Body;
            emailMessage.Body = builder.ToMessageBody();
            return emailMessage;

        }
    }
}
