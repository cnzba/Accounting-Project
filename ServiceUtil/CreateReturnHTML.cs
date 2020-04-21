using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceUtil
{
    public class CreateReturnHTML
    {
        public byte[] GetHTML(string body)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();
            htmlStringBuilder.Append("<html>");
            htmlStringBuilder.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> </head>");
            htmlStringBuilder.Append("<body>");
            htmlStringBuilder.Append($"{body}");
            htmlStringBuilder.Append("</body>");
            htmlStringBuilder.Append("</html>");
            var data = Encoding.UTF8.GetBytes(htmlStringBuilder.ToString());
            return data;

        }
    }
}
