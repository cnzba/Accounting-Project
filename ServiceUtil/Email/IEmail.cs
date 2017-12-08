using System.Collections.Generic;

namespace ServiceUtil.Email
{
    public interface IEmail
    {
        List<string> Attachment { get; set; }
        string Body { get; set; }
        List<string> Cc { get; set; }
        List<string> Bco { get; set; }        
        string Subject { get; set; }
        string To { get; set; }
    }
}