﻿using Core.Entities.OrderAggregate;

namespace ApiDemo.Dtos
{
    public class OrderItemDto
    { 
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
}
}