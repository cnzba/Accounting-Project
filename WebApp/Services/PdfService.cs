using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using ServiceUtil.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics; // remember to remove this , used for debug.console()
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Net;


namespace WebApp.Services
{
    public class PdfService : IPdfService
    {
        private readonly IEmailService emailService;
        private readonly IConverter converter;
        private readonly EmailConfig emailConfig;
        private readonly PdfServiceOptions serviceConfig;
        private readonly CBAContext context;

        public PdfService(IEmailService emailService,
                            IOptionsSnapshot<EmailConfig> emailConfig,
                            IOptionsSnapshot<PdfServiceOptions> serviceConfig,
                            IConverter converter,
                            CBAContext context)
        {
            this.emailService = emailService;
            this.converter = converter;
            this.emailConfig = emailConfig.Value;
            this.serviceConfig = serviceConfig.Value;
            this.context = context;
        }

        private string GetDestination(string InvoiceNumber)
        {
            var destination = Path.GetTempPath();
            destination = Path.Combine(destination, InvoiceNumber + ".pdf");

            return destination;
        }

        /// <summary>
        /// A stub that copies the invoice_sample pdf. This file should be removed
        /// from the project when this stub is replaced with "real" code
        /// </summary>
        /// <param name="InvoiceNumber">The invoice to create</param>
        /// <returns>The filename/path of the generated pdf</returns>
        private string CreatePdf(string invoiceNumber)
        {
            try
            {
                DeletePdf(invoiceNumber);
                var invoice = context.Invoice.Include("InvoiceLine").SingleOrDefault(t => t.InvoiceNumber == invoiceNumber);
                
                string charitiesNumber = invoice.CharitiesNumber;
                //string clientContact = invoice.ClientContact; // address, replace \n with <br> for html
                var clientContact = Regex.Replace(invoice.ClientContact, @"\r\n?|\n", "<br />");

                string clientName = invoice.ClientName; // name
                DateTime dateCreated = invoice.DateCreated; // Invoice Date
                DateTime dateDue = invoice.DateDue;
                string email = invoice.Email;
                decimal grandTotal = invoice.GrandTotal;
                string gstNumber = invoice.GstNumber;
                decimal gstRate = invoice.GstRate;
                var subTotal = invoice.SubTotal;
                var invoiceLine = invoice.InvoiceLine; // collection of the item in description
                string purchaseOrderNumber = invoice.PurchaseOrderNumber;

                // there are more fields
                XDocument temp = XDocument.Load("Services/PdfServiceHtmlModel.html");
                //temp.Descendants().Where(x => (string)x.Attribute("id") == "myidtest").FirstOrDefault().Value = "foobar"; // .Where(x => (Guid?) x.Attribute("id") == id).FirstOrDefault(); for null?
                temp.Descendants().Where(x => (string)x.Attribute("id") == "clientName").FirstOrDefault().Value = clientName;


                var clientContactDetails = XElement.Parse("<span>" + clientContact + "</span>");
                temp.Descendants().Where(x => (string)x.Attribute("id") == "clientContact").FirstOrDefault().Add(clientContactDetails);


                // https://stackoverflow.com/questions/19271080/c-sharp-xml-avoid-html-encode-using-xdocument
                temp.Descendants().Where(x => (string)x.Attribute("id") == "invoiceNumber").FirstOrDefault().Value = invoiceNumber;
                temp.Descendants().Where(x => (string)x.Attribute("id") == "dateCreated").FirstOrDefault().Value = dateCreated.ToString();
                temp.Descendants().Where(x => (string)x.Attribute("id") == "gstNumber").FirstOrDefault().Value = gstNumber;
                temp.Descendants().Where(x => (string)x.Attribute("id") == "charitiesNumber").FirstOrDefault().Value = charitiesNumber;
                temp.Descendants().Where(x => (string)x.Attribute("id") == "purchaseOrderNumber").FirstOrDefault().Value = purchaseOrderNumber;
 
                temp.Descendants().Where(x => (string)x.Attribute("id") == "dateDue").FirstOrDefault().Value = "Due Date" + dateDue.ToString();

                temp.Descendants().Where(x => (string)x.Attribute("id") == "gstRate").FirstOrDefault().Value = String.Format("GST {0}%", gstRate * 100);

                var gstValue = grandTotal - subTotal;
                temp.Descendants().Where(x => (string)x.Attribute("id") == "gstValue").FirstOrDefault().Value = gstValue.ToString();
                temp.Descendants().Where(x => (string)x.Attribute("id") == "subTotal").FirstOrDefault().Value = subTotal.ToString();
                temp.Descendants().Where(x => (string)x.Attribute("id") == "grandTotal").FirstOrDefault().Value = grandTotal.ToString();

                string holder = "<tr><td>{0}</td><td style=\"text-align: right\">{1}</td><td style=\"text-align: right\">{2}</td><td style=\"text-align: right\">{3}</td></tr>";
                foreach (var item in invoiceLine)
                {
                    var testNode = XElement.Parse(String.Format(holder, item.Description, item.Quantity, item.UnitPrice, item.Amount));
                    temp.Descendants().Where(x => (string)x.Attribute("id") == "testTable").FirstOrDefault().Add(testNode);
                }
                
                
                
                // Debug.WriteLine(charitiesNumber); 
                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10 }
                },
                    Objects = {
                    new ObjectSettings()
                    {
                       HtmlContent = temp.ToString(),
                    }
                }
                };

                byte[] pdf = converter.Convert(doc);

                var destination = GetDestination(invoiceNumber);
                File.WriteAllBytes(destination, pdf);

                return destination;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }   
        }

        /// <summary>
        /// As pdfs are created into temporary, limited storage they need to be cleaned up.
        /// </summary>
        /// <param name="InvoiceNumber"></param>
        private void DeletePdf(string InvoiceNumber)
        {
            var filePath = GetDestination(InvoiceNumber);

            File.Delete(filePath);
        }

        public async Task EmailPdf(Invoice invoice)
        {
            var attachment = CreatePdf(invoice.InvoiceNumber);

            var pathToTemplate = Path.Combine("EmailTemplates", serviceConfig.DefaultTemplate);
            string htmlBody = File.ReadAllText(pathToTemplate);

            Email emailContent = new Email()
            {
                To = invoice.Email,
                Subject = $"Invoice #{invoice.InvoiceNumber}",
                Body = String.Format(htmlBody, invoice.ClientName, invoice.InvoiceNumber)
            };

            emailContent.Attachment.Add(attachment);

            bool success = await emailService.SendEmail(emailConfig, emailContent);
            if (!success) throw new PdfServiceException("The invoice could not be emailed.") { InvoiceNumber = invoice.InvoiceNumber };

            DeletePdf(invoice.InvoiceNumber);
        }

        public string GetPdfInvoice(string invoiceNumber)
        {
            return this.CreatePdf(invoiceNumber);
        }
    }
}
