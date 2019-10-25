using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class PdfServiceException : Exception
    {
        public string InvoiceNumber { get; set; }

        public PdfServiceException()
        {

        }

        public PdfServiceException(string message) : base(message)
        {

        }

        public PdfServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
