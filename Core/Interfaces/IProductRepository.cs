﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        public Task<IReadOnlyList<ProductBrand>>  GetBrandsAsync();
        public Task<IReadOnlyList<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<IReadOnlyList<ProductType>> GetProductTypesAsync();

    }
}
