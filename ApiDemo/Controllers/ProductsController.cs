using ApiDemo.Dtos;
using ApiDemo.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiDemo.Controllers
{
   
    public class ProductsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<ProductBrand> productBrandRepository;
        private readonly IGenericRepository<ProductType> productTypeRepository;


        public ProductsController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType>productTypeRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.productBrandRepository = productBrandRepository;
            this.productTypeRepository = productTypeRepository;
        }
        [Cached(10)]
        [HttpGet("GetProducts")]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecificParams productSpecificParams)
        {
            var spec=new ProductsWithBrandAndTypeSpecification(productSpecificParams);
            var countSpec=new ProductWithFiltersForCountSpecifications(productSpecificParams);
            var totalItems =await unitOfWork.Repository<Product>().CountAsync(countSpec);
            var products = await unitOfWork.Repository<Product>().ListAsync(spec);
            var mappedProducts= mapper.Map<IReadOnlyList<ProductDto>>(products);
            var paginatedData = new Pagination<ProductDto>(productSpecificParams.PageIndex, totalItems, productSpecificParams.PageSize, mappedProducts);
            return Ok(paginatedData);

        }
        [HttpGet("GetBrands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await productBrandRepository.GetAllAsync());
        }
        [HttpGet("GetProduct")]

        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var spec = new ProductsWithBrandAndTypeSpecification(id);


           var product= await unitOfWork.Repository<Product>().GetEntityWithSpecification(spec);
            var mappedProduct = mapper.Map<ProductDto>(product);
            if(mappedProduct is null)
            {
                return NotFound();
            }
            return Ok(mappedProduct);
        }
        [HttpGet("GetProductTypes")]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await productTypeRepository.GetAllAsync());
        }
    }
}
