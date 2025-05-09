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
    public class ImportInvoiceDetailConfiguration : IEntityTypeConfiguration<ImportInvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<ImportInvoiceDetail> builder)
        {
            builder.ToTable("ImportInvoiceDetail");

            builder.Property(d => d.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(d => d.TotalAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.ImportInvoice)
                .WithMany(i => i.Details)
                .HasForeignKey(d => d.ImportInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId);

            builder.HasMany(d => d.InventoryBatches)
            .WithOne(b => b.ImportInvoiceDetail)
            .HasForeignKey(b => b.ImportInvoiceDetailId)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }

}
