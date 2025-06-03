using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Infrastructure.Configuration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Phone)
                   .HasMaxLength(20);

            builder.Property(s => s.Email)
                   .HasMaxLength(100);

            builder.Property(s => s.Address)
                   .HasMaxLength(200);

            builder.Property(p => p.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.UpdateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CreateBy)
                .HasDefaultValue(0);

            builder.Property(p => p.UpdateBy)
                .HasDefaultValue(0);
        }


    }
}
