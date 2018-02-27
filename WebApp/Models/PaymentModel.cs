using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class PaymentModel
    {
        [JsonIgnore]
        [BindNever]
        public int Id { get; set; }
        public string Token { get; set; }
        public string TokenId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string InvoiceNo { get; set; }
        public string Charge { get; set; }
        public string ChargeId { get; set; }
        public string RefId { get; set; }
        public string Type { get; set; }
        public string Gateway { get; set; }
    }
}
