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
    public class ProductUnitConfiguration : IEntityTypeConfiguration<ProductUnit>
    {
        public void Configure(EntityTypeBuilder<ProductUnit> builder)
        {
            builder.ToTable("ProductUnit");

            builder.HasKey(pu => pu.Id);

            builder.Property(pu => pu.Unit)
                .IsRequired()
                .HasMaxLength(50); // Chuỗi cho đơn vị

            builder.Property(pu => pu.ConversionFactor)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(pu => pu.Product)
                .WithMany(p => p.ProductUnits)
                .HasForeignKey(pu => pu.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pu => new { pu.ProductId, pu.Unit })
                .IsUnique(); // Đảm bảo không trùng đơn vị trong cùng sản phẩm
        }
    }
}
