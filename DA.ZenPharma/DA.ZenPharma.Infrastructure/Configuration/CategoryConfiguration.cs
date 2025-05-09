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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.CategoryDescription)
                .HasMaxLength(1000);

            builder.Property(c => c.ImagePath)
                .HasMaxLength(255);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);
        }
    }

}
