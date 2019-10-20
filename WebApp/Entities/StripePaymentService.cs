using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using WebApp.Services;

namespace WebApp.Entities
{
    public class StripePaymentService : IStripePaymentService
    {
        private CBAContext _context;
        private IInvoiceService _invoiceService;
        private IHostingEnvironment _env;
        private ILogger _logger;

        public StripePaymentService(CBAContext context, IInvoiceService invoiceService, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _context = context;
            _invoiceService = invoiceService;
            _env = env;
            _logger = loggerFactory.CreateLogger<StripePaymentService>();
        }

        public PaymentResponse ChargeCard(PaymentRequest paymentRequest)
        {
            PaymentResponse response = new PaymentResponse();
            Invoice invoice = _invoiceService.GetInvoiceByPaymentId(paymentRequest.PaymentId);

            // TODO add logic for repayment

            if (invoice == null)
            {
                response.Status = "failed";
                response.Message = "Invalid invoice number";
            } else if (invoice.Status == InvoiceStatus.Sent)
            {
                // TODO Change test key
                string apiKey = _env.IsDevelopment() ? "sk_test_bt2EhY73T2WBSgIi5ukAMKjx" : "sk_test_bt2EhY73T2WBSgIi5ukAMKjx";
                StripeConfiguration.SetApiKey(apiKey);

                var myCharge = new StripeChargeCreateOptions();
                myCharge.SourceTokenOrExistingSourceId = paymentRequest.TokenId;
                myCharge.Amount = Decimal.ToInt32(invoice.GrandTotal * 100);
                myCharge.Currency = "nzd";
                myCharge.Description = "Invoice No - " + invoice.InvoiceNumber;
                myCharge.Metadata = new Dictionary<string, string>();
                myCharge.Metadata["PaymentId"] = paymentRequest.PaymentId;

                var chargeService = new StripeChargeService();
                StripeCharge stripeCharge = chargeService.Create(myCharge);

                // TODO Handle messages better. Seller message is for devs, not customers
                response.Status = stripeCharge.Status;
                response.Message = stripeCharge.Outcome.SellerMessage;

                PaymentModel paymentModel = new PaymentModel()
                {
                    Amount = invoice.GrandTotal,
                    Charge = JsonConvert.SerializeObject(stripeCharge),
                    ChargeId = stripeCharge.Id,
                    Type = paymentRequest.Type,
                    Gateway = paymentRequest.Gateway,
                    InvoiceNo = invoice.InvoiceNumber,
                    Status = stripeCharge.Status,
                    Token = paymentRequest.TokenObj,
                    TokenId = paymentRequest.TokenId,
                    PaymentId = paymentRequest.PaymentId,
                    paymentDate = DateTime.Now
                };

                _context.Add<PaymentModel>(paymentModel);

                if (response.Status.Equals("succeeded"))
                {
                    // TODO Update invoice status, send email
                    invoice.Status = InvoiceStatus.Paid;
                }

                _context.SaveChanges();
            } else if (invoice.Status == InvoiceStatus.Paid || invoice.Status == InvoiceStatus.Cancelled)
            {
                response.Status = "failed";
                response.Message = "Invoice has already been paid";
            } else if (invoice.Status == InvoiceStatus.Draft || invoice.Status == InvoiceStatus.New)
            {
                response.Status = "failed";
                response.Message = "Invoice has not been sent";
            }

            return response;
        }
    }
}
