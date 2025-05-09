using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Infrastructure.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");

            builder.Property(d => d.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(d => d.TotalAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId);

            builder.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId);
        }
    }

}
