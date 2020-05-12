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

using System.Xml.Linq;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WebApp.Services
{
    public class PdfService : IPdfService
    {
        private readonly IEmailService emailService;
        private readonly IConverter converter;
        private readonly IViewRenderService viewRenderService;
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
            this.viewRenderService = viewRenderService;
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

                var charitiesNumber = invoice.CharitiesNumber; // string
                var clientContact = Regex.Replace(invoice.ClientContact, @"\r\n?|\r\n|\n", "<br />"); // string, replace "\n" with "<br/>" for html
                var clientName = invoice.ClientName; // string
                var dateCreated = invoice.DateCreated; // DateTime
                var dateDue = invoice.DateDue; // DateTime
                var email = invoice.Email; // string
                var grandTotal = invoice.GrandTotal; // decimal
                var gstNumber = invoice.GstNumber;  // string
                var gstRate = invoice.GstRate; // decimal
                var subTotal = invoice.SubTotal; // decimal
                var invoiceLine = invoice.InvoiceLine; // ICollection<InvoiceLine> of the items
                var purchaseOrderNumber = invoice.PurchaseOrderNumber; // string

                // there are more fields but most are null or missing i.e. no logo field?
                XDocument document = XDocument.Load("Services/PdfServiceHtmlModel.html");

                // var logoUrl = "https://www.childhood.org.au/app/uploads/2017/07/ACF-logo-placeholder.png";
                // document.Descendants().Where(x => (string)x.Attribute("id") == "logo").FirstOrDefault().Add(XElement.Parse(string.Format("<img src=\"{0}\" alt=\"\" width=\"50\" height=\"50\"/>", logoUrl)));
                // uncomment above line when there is an logo field, set the src to a url or file directory
                AddField(ref document, "clientName", clientName);
            
                AddFieldWithNewline(ref document, "clientContact", clientContact.ToString());

                AddField(ref document, "invoiceNumber", invoiceNumber.ToString());
                AddField(ref document, "dateCreated", dateCreated.ToString());
                AddField(ref document, "gstNumber", gstNumber.ToString());
                AddField(ref document, "charitiesNumber", charitiesNumber.ToString());
                AddField(ref document, "purchaseOrderNumber", purchaseOrderNumber.ToString());
                AddField(ref document, "dateDue", "Due Date " + dateDue.ToString());
                AddField(ref document, "gstRate", string.Format("GST {0}%", gstRate * 100));

                var gstValue = grandTotal - subTotal;
                AddField(ref document, "gstValue", gstValue.ToString());
                AddField(ref document, "subTotal", subTotal.ToString());
                AddField(ref document, "grandTotal", subTotal.ToString());

                // populate the table
                var holder = "<tr><td>{0}</td><td style=\"text-align: right\">{1}</td><td style=\"text-align: right\">{2}</td><td style=\"text-align: right\">{3}</td></tr>";
                foreach (var item in invoiceLine)
                {
                    var node = XElement.Parse(string.Format(holder, item.Description, item.Quantity, item.UnitPrice, item.Amount));
                    document.Descendants().Where(x => (string)x.Attribute("id") == "invoiceTable").FirstOrDefault().Add(node);
                }

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
                       HtmlContent = document.ToString(),
                    }
                }
                };

                byte[] pdf = converter.Convert(doc);

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false); // font
                pdf = AddWatermark(pdf, bfTimes, "Copy");

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
        /// This method adds data to an html tag by id
        /// </summary>
        private static void AddField(ref XDocument document, string id, string data) 
        {
            document.Descendants().Where(x => (string)x.Attribute("id") == id).FirstOrDefault().Value = data;
        }

        /// <summary>
        /// Similar to AddField, use if string had any inline tags. There is a weird quirk 
        /// that when adding to the html tag by .Value; inline <> tags are not preserved so when trying to display a newline 
        /// via <br/> it insteads display "<br/>" literally. It might have something to do with ascii codes?
        /// to remedy this use .Add() and wrap the XELement in a <span>
        /// </summary>
        private static void AddFieldWithNewline(ref XDocument document, string id, string data)
        {
            document.Descendants().Where(x => (string)x.Attribute("id") == id).FirstOrDefault()
                .Add(XElement.Parse("<span>" + data + "</span>"));
        }

        /// <summary>
        /// This method adds watermark text under pdf content
        /// </summary>
        /// <param name="pdfData">pdf content bytes</param>
        /// <param name="watermarkText">text to be shown as watermark</param>
        /// <param name="font">base font</param>
        /// <param name="fontSize">font size</param>
        /// <param name="angle">angle at which watermark needs to be shown in degrees</param>
        /// <param name="color">water mark color</param>
        /// <param name="realPageSize">pdf page size</param>
        private static void AddWaterMarkText(PdfContentByte pdfData, string watermarkText, BaseFont font, float fontSize, float angle, BaseColor color, Rectangle realPageSize)
        {
            var gstate = new PdfGState { FillOpacity = 0.35f, StrokeOpacity = 0.3f };
            pdfData.SaveState();
            pdfData.SetGState(gstate);
            pdfData.SetColorFill(color);
            pdfData.BeginText();
            pdfData.SetFontAndSize(font, fontSize);
            var x = (realPageSize.Right + realPageSize.Left) / 2;
            var y = (realPageSize.Bottom + realPageSize.Top) / 2;
            pdfData.ShowTextAligned(Element.ALIGN_CENTER, watermarkText, x, y, angle);
            pdfData.EndText();
            pdfData.RestoreState();
        }

        /// <summary>
        /// This method calls another method to add watermark text for each page
        /// </summary>
        /// <param name="bytes">byte array of Pdf</param>
        /// <param name="baseFont">Base font</param>
        /// <param name="watermarkText">Text to be added as watermark</param>
        /// <returns>Pdf bytes array having watermark added</returns>
        private static byte[] AddWatermark(byte[] bytes, BaseFont baseFont, string watermarkText)
        {
            using (var ms = new MemoryStream(10 * 1024))
            {
                using (var reader = new PdfReader(bytes))
                using (var stamper = new PdfStamper(reader, ms))
                {
                    var pages = reader.NumberOfPages;
                    for (var i = 1; i <= pages; i++)
                    {
                        var dc = stamper.GetOverContent(i);
                        AddWaterMarkText(dc, watermarkText, baseFont, 100, 45, BaseColor.GRAY, reader.GetPageSizeWithRotation(i));
                    }
                    stamper.Close();
                }
                return ms.ToArray();
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
