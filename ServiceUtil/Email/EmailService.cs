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
        public async Task<bool> SendEmail(IEmail email)
        {
            try
            {
                MimeMessage emailMessage = CreateEmailBody(email);

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = email.EmailConfig.LocalDomain;
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(email.EmailConfig.MailServerAddress, Convert.ToInt32(email.EmailConfig.MailServerPort), SecureSocketOptions.StartTls).ConfigureAwait(false);

                    await client.AuthenticateAsync(new NetworkCredential(email.EmailConfig.UserId, email.EmailConfig.UserPassword));
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

        private MimeMessage CreateEmailBody(IEmail email)
        {
            var emailMessage = new MimeMessage();
            var builder = new BodyBuilder();

            emailMessage.From.Add(new MailboxAddress(email.EmailConfig.FromName, email.EmailConfig.FromAddress));
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
