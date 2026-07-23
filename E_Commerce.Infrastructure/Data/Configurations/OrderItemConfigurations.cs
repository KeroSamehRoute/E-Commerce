using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configurations;

internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {

        builder.ToTable("OrderItems");

        builder.Property(o => o.Price)
            .HasColumnType("decimal(10,2)");

        builder.OwnsOne(o => o.Product);

    }
}
