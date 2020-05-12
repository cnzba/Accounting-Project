using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceUtil
{
    public interface ICreateReturnHTML
    {
        byte[] GetHTML(string body);
    }
}
