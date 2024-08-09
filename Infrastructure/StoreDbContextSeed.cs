using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

using System.Text.Json;

namespace Infrastructure
{
    public class StoreDbContextSeed
    {
        public static async Task Seed(StoreDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await SeedBrands(context);
                await SeedTypes(context);
                await SeedProducts(context);
                await SeedDeliveryMethods(context);
            }
            catch (Exception ex) {
              var logger=  loggerFactory.CreateLogger<StoreDbContextSeed>();
                logger.LogError(ex.Message);
            }
          
        }
        public static async Task SeedBrands(StoreDbContext context)
        {
            if (context.ProductBrands != null && !context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                foreach (var brand in brands)
                    context.ProductBrands.Add(brand);
                
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedTypes(StoreDbContext context)
        {
            if (context.ProductTypes != null && !context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                foreach (var type in types)
                    context.ProductTypes.Add(type);
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedProducts(StoreDbContext context)
        {
            if (context.Products != null && !context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                foreach (var product in products)
                    context.Products.Add(product);
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedDeliveryMethods(StoreDbContext context)
        {
            if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                foreach (var deliveryMethod in deliveryMethods)
                    context.DeliveryMethods.Add(deliveryMethod);
                await context.SaveChangesAsync();
            }
        }
    }
}
