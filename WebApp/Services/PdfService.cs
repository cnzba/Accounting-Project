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

namespace WebApp.Services
{
    public class PdfService : IPdfService
    {
        private readonly IEmailService emailService;
        private readonly IConverter converter;
        private readonly EmailConfig emailConfig;
        private readonly PdfServiceOptions serviceConfig;
        private InvoiceService invoiceService;

        public PdfService(IEmailService emailService,
                            IOptionsSnapshot<EmailConfig> emailConfig,
                            IOptionsSnapshot<PdfServiceOptions> serviceConfig,
                            IConverter converter)
        {
            this.emailService = emailService;
            this.converter = converter;
            this.emailConfig = emailConfig.Value;
            this.serviceConfig = serviceConfig.Value;
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
        private string CreatePdf(string InvoiceNumber)
        {
            try
            {
                DeletePdf(InvoiceNumber);
                var temp = invoiceService.GetInvoice(InvoiceNumber);
                Console.WriteLine(temp);
                //
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
                       HtmlContent = @"<html><body><div>Hello</div></body></html>",
                    }
                }
                };

                byte[] pdf = converter.Convert(doc);

                var destination = GetDestination(InvoiceNumber);
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
