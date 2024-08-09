using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public  void Configure(EntityTypeBuilder<OrderItem> builder)
        {
             builder.OwnsOne(item => item.Product, itemOrdered => itemOrdered.WithOwner());
             //throw new NotImplementedException();

        }
    }
}
