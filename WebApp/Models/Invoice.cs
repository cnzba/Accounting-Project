using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceLine = new HashSet<InvoiceLine>();
        }

        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public string IssueeOrganization { get; set; }
        public string IssueeCareOf { get; set; }
        public int Gstnumber { get; set; }
        public int? CharitiesNumber { get; set; }

        public ICollection<InvoiceLine> InvoiceLine { get; set; }
    }
}
