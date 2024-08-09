using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecifications:BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecifications(ProductSpecificParams productSpecParams)
          : base(product =>
          (string.IsNullOrEmpty(productSpecParams.Search) || product.Name.ToLower().Contains(productSpecParams.Search)) &&
          (!productSpecParams.ProductBrandId.HasValue || product.ProductBrandId == productSpecParams.ProductBrandId)
          && (!productSpecParams.ProductTypeId.HasValue || product.ProductTypeId == productSpecParams.ProductTypeId)) { }
    }
}
