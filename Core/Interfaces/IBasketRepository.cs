using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket> GetBasketAsync(string BasketId);
        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        public Task DeleteBasketAsync(string BasketId);

    }
}
