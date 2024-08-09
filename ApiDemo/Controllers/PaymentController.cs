using ApiDemo.ResponseModule;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ApiDemo.Controllers
{
   
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> logger;
        private const string whSecret= "whsec_47e6aeba00844b1701c41c054db463a890f4f6d4714f0030995c516604ec43f8";

        
        public PaymentController(IPaymentService paymentService,ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }
        [HttpPost("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string BasketId)
        { 
            var basket=await paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (basket is null)
                return BadRequest(new ApiResponse(400, "Problem with payment"));
            return Ok(basket);
        }
        [HttpPost("webhook")]
        public async Task<ActionResult<CustomerBasket>> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            ///
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], whSecret);
               // var stripeEvent = EventUtility.ParseEvent(json);

                PaymentIntent intent;
                Order order;
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        logger.LogInformation("Payment Succeed");
                        order = await paymentService.UpdateOrderPaymentSucceed(intent.Id);
                        logger.LogInformation("Payment Succeed");

                        break;
                    case Events.PaymentIntentPaymentFailed:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        logger.LogInformation("Payment Failed");
                        order = await paymentService.UpdateOrderPaymentFailed(intent.Id);
                        logger.LogInformation("Payment Failed");

                        break;
                }
                return Ok();
            }catch(StripeException)
            {
                return  BadRequest();

            }

        }
    }
}
