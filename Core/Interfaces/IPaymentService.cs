using Core.Entities;
using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        public Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        public Task<Order> UpdateOrderPaymentSucceed(string PaymentIntentId);
        public Task<Order> UpdateOrderPaymentFailed(string PaymentIntentId);
    }
}
