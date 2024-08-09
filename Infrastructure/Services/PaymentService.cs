using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Stripe;
//using Stripe;


namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;

        public PaymentService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IBasketRepository basketRepository)
        {
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            Stripe.StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            var basket = await basketRepository.GetBasketAsync(basketId);
            if(basket is null)
                return null;
            var shippingPrice =0m;

            if (basket.DeliveryMethod.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethod.Value);
                 shippingPrice = deliveryMethod.Price;

            }
            foreach (var item in basket.BasketItems)
            {
                var productItem =await unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }
            var service = new PaymentIntentService();
            PaymentIntent intent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(basket.BasketItems.Sum(item => item.quantity * (item.Price * 100)) +shippingPrice * 100),
                    Currency= "usd",
                    PaymentMethodTypes=new List<string> { "card"}               
                };
                intent =await service.CreateAsync(options);
                
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(basket.BasketItems.Sum(item => item.quantity * (item.Price * 100)) + shippingPrice * 100),
                  
                };
               await service.UpdateAsync(basket.PaymentIntentId, options);
                
            }
           await basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order =await unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
            if (order is null)
                return null;
            order.OrderStatus = OrderStatus.PaymentFaied;
            unitOfWork.Repository<Order>().Update(order);
            await unitOfWork.Complete();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceed(string paymentIntentId)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
            if (order is null)
                return null;
            order.OrderStatus = OrderStatus.PaymentReceived;
            unitOfWork.Repository<Order>().Update(order);
            await unitOfWork.Complete();
            return order;
        }
    }
}
