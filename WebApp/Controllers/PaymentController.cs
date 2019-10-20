using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Entities;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private IStripePaymentService _stripePaymentService;

        public PaymentController(IStripePaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostChargeCard([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }

            if (request.Gateway.Equals("stripe"))
            {
                return Json(_stripePaymentService.ChargeCard(request));
            }

            return BadRequest("Please select payment gateway");
        }
    }
}
