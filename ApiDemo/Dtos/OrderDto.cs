using Core.Entities.OrderAggregate;

namespace ApiDemo.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }

        public ShippingAddressDto ShippedToAddress { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
