using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;

namespace WebApp.Services
{
    public interface IPdfService
    {
        Task EmailPdf(Invoice invoiceToSend);
        string getPdfInvoice(string invoiceNumber);
    }
}
