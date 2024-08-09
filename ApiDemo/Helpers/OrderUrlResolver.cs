using ApiDemo.Dtos;
using AutoMapper;
using Core.Entities.OrderAggregate;

namespace ApiDemo.Helpers
{
    public class OrderUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return configuration["ApiUrl"] + source.Product.PictureUrl;
            return null;

        }
    }
}
