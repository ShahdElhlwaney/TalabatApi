using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(a => a.ShippedToAddress, a => a.WithOwner());
            builder.Property(p=>p.OrderStatus)
                .HasConversion(
                status=> status.ToString(),
                value=>(OrderStatus) Enum.Parse(typeof(OrderStatus),value)
                );
            builder.HasMany(p=>p.orderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
