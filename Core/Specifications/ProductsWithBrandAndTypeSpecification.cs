using Core.Entities;


namespace Core.Specifications
{
    public class ProductsWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypeSpecification(ProductSpecificParams productSpecParams)
            : base(product=>
            (string.IsNullOrEmpty(productSpecParams.Search)|| product.Name.Contains(productSpecParams.Search))&&
            (!productSpecParams.ProductBrandId.HasValue || product.ProductBrandId==productSpecParams.ProductBrandId)
            && (!productSpecParams.ProductTypeId.HasValue || product.ProductTypeId == productSpecParams.ProductTypeId))
        {
            AddInclude(product => (product.ProductBrand));
            AddInclude(Product => (Product.ProductType));
            AddOrderBy(Product => Product.Name);
            ApplyPaging(productSpecParams.PageSize*(productSpecParams.PageIndex-1),productSpecParams.PageSize);
            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "priceAsc" :
                        AddOrderBy(Product => Product.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(Product => Product.Price);
                        break;
                    default:
                        AddOrderBy(Product => Product.Name);
                        break;
                }
            }
            
        }
        public ProductsWithBrandAndTypeSpecification(int id):base(product=>product.Id==id)
        {
            AddInclude(product => (product.ProductBrand));
            AddInclude(Product => (Product.ProductType));
        }
    }
}
