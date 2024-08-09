﻿using Core.Entities.OrderAggregate;

namespace ApiDemo.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public ShippingAddressDto ShippedToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public IReadOnlyList<OrderItemDto> orderItems { get; set; }
        public decimal SubTotal { get; set; }
        public string OrderStatus { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingPrice { get;set; }
        public string PaymentIntentId { get; set; }

    }
}
