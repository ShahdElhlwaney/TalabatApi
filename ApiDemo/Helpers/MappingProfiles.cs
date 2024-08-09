using ApiDemo.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
namespace ApiDemo.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destination => destination.ProductBrand
                , option => option.MapFrom(src => src.ProductBrand.Name))
                .ForMember(destination => destination.ProductType
                , option => option.MapFrom(src => src.ProductType.Name))
                .ForMember(destination => destination.PictureUrl
               , option => option.MapFrom<ProductUrlResolver>());
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();
            CreateMap<Order, OrderDetailsDto>().ForMember(dest => dest.DeliveryMethod,
                option => option.MapFrom(src => src.DeliveryMethod.ShortName)
            ).ForMember(dest => dest.ShippingPrice,
               option => option.MapFrom(src => src.DeliveryMethod.Price)
           );


            CreateMap<OrderItem, OrderItemDto>().ForMember(dest => dest.ProductItemId,
             option => option.MapFrom(src => src.Product.ProductItemId)
         ).ForMember(dest => dest.ProductName,
            option => option.MapFrom(src => src.Product.ProductName)
        ).ForMember(dest => dest.ProductName,
            option => option.MapFrom(src => src.Product.ProductName)
        ).ForMember(dest => dest.PictureUrl,
            option => option.MapFrom(src => src.Product.PictureUrl)
        ).ForMember(dest => dest.PictureUrl,
            option => option.MapFrom<OrderUrlResolver>()
        );///

        }
    }
}
