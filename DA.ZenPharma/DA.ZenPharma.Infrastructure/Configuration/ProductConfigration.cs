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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.RegularPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DiscountPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false); 

            builder.Property(p => p.ThumbnailImagePath)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(p => p.Description)
                .HasMaxLength(4000)
                .IsRequired(false);

            builder.Property(p => p.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.UpdateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CreateBy)
                .HasDefaultValue(0);

            builder.Property(p => p.UpdateBy)
                .HasDefaultValue(0);

            builder.Property(p => p.IsPublished)
                .HasDefaultValue(true);

            builder.Property(p => p.IsPrescriptionRequired)
                .HasDefaultValue(false);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProductUnits)
                .WithOne(pu => pu.Product)
                .HasForeignKey(pu => pu.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.ProductCode)
                .IsUnique();
        }

    }

}
