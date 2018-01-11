using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceUtil.Email
{
    public class EmailConfig : IEmailConfig
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }

        public string LocalDomain { get; set; }

        public string MailServerAddress { get; set; }
        public int MailServerPort { get; set; }

        public string UserId { get; set; }
        public string UserPassword { get; set; }
    }
}
