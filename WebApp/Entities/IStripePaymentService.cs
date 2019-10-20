using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    public interface IStripePaymentService
    {
        PaymentResponse ChargeCard(PaymentRequest paymentRequest);
    }
}
