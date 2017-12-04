﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceUtil.Email
{
    public class Email : IEmail
    {
        public Email()
        {
            EmailConfig = new EmailConfig();
            Attachment = new List<string>();
            Cc = new List<string>();
            Bco = new List<string>();
        }

        public IEmailConfig EmailConfig { get; set; }
        public string To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bco { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachment { get; set; }
        
    }
}
