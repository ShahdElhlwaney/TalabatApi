﻿

using ApiDemo.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ApiDemo.Controllers
{
    [Authorize]
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper) {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
           var basket= await basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>  UpdateBasket(CustomerBasketDto customerBasketDto)
        {
          var basket=  mapper.Map<CustomerBasket>(customerBasketDto);
           var updatedBasket=await basketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await basketRepository.DeleteBasketAsync(id);
        }

    }
}
