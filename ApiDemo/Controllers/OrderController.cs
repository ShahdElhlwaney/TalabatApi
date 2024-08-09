using ApiDemo.Dtos;
using ApiDemo.Extensions;
using ApiDemo.ResponseModule;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiDemo.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            
            var address = mapper.Map<ShippingAddress>(orderDto.ShippedToAddress);
            var email = User.RetrieveEmailFromPrincipal();
            var order = await orderService.CreateOrderAsync(orderDto.BasketId, orderDto.DeliveryMethodId, email, address);
            if (order == null)
            {
                return BadRequest(new ApiResponse(404, "Problem when Creating Order !!"));
            }
            return Ok(order);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdForUser(int id)
        {
            var email= User.RetrieveEmailFromPrincipal();
           var order= await orderService.GetOrderByIdAsync(id, email);
            if (order is null)
                return NotFound(new ApiResponse(404, "Order does not exist"));
            return Ok(mapper.Map<OrderDetailsDto>(order));
        }
        [HttpGet("GetOrdersForUser")]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetOrdersForUser()
        {
            var email = User.RetrieveEmailFromPrincipal();
            var orders =await orderService.GetOrdersForUserAsync(email);
            return Ok(mapper.Map<IReadOnlyList<OrderDetailsDto>>(orders));
        }
        [HttpGet("GetDeliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {      
            return Ok(await orderService.GetDeliveryMethods());
    }
}
}

