using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceUtil.Email
{
    public class EmailConfig : IEmailConfig
    {
        public String FromName { get; set; }
        public String FromAddress { get; set; }

        public String LocalDomain { get; set; }

        public String MailServerAddress { get; set; }
        public int MailServerPort { get; set; }

        public String UserId { get; set; }
        public String UserPassword { get; set; }
    }
}
