using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;
        private readonly IPaymentService paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository,IPaymentService paymentService)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
            this.paymentService = paymentService;
        }




        public async Task<Order> CreateOrderAsync(string basketId, int deliveryMethodId, string buyerEmail, ShippingAddress address)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productOrdered, product.Price, item.quantity);
                items.Add(orderItem);
            }
            var deliveryMethod=await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var total= items.Sum(i=>i.Price*i.Quantity);
            ///ToDo Payment
            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var existingOrder =await unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
            if(existingOrder !=null)
            {
                unitOfWork.Repository<Order>().Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basketId);

            }
            var order = new Order(buyerEmail, address, deliveryMethod, items, total);
             unitOfWork.Repository<Order>()
                .Add(order);
            var result=await unitOfWork.Complete();
            if (result <= 0)
                return null;
            return order;

        }
       


        
    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
        {
            return await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsSpecification(id, buyerEmail);
           return await unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsSpecification(buyerEmail);
            return await unitOfWork.Repository<Order>().ListAsync(spec);
        }

        
    }
}





