using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class PdfServiceOptions
    {
        public PdfServiceOptions() {}
        public string DefaultTemplate { get; set; } // set the template within the EmailTemplates directory

        // If OverrideIssueEmail is true then invoices won't be issued to the email address on the invoice.
        // Instead, if OverrideEmail is provided that is used instead. Otherwise, no email is sent.
        public bool OverrideIssueEmail { get; set; } 
        public string OverrideEmail { get; set; } 
    }
}
