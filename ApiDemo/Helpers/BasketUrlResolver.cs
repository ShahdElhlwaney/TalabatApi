using ApiDemo.Dtos;
using AutoMapper;
using Core.Entities;

namespace ApiDemo.Helpers
{
    public class BasketUrlResolver : IValueResolver<BasketItem, BasketItemDto, string>
    {
        private readonly IConfiguration configuration;

        public BasketUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(BasketItem source, BasketItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return configuration["ApiUrl"] + source.PictureUrl;
            return null;
        }
    }
}
