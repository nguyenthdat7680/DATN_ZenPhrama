using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DA.ZenPharma.Infrastructure.Configuration
{
    public class InventoryBatchConfiguration : IEntityTypeConfiguration<InventoryBatch>
    {
        public void Configure(EntityTypeBuilder<InventoryBatch> builder)
        {
            builder.ToTable("InventoryBatch");

            builder.Property(b => b.LocationCode).HasMaxLength(100);

            builder.HasOne(b => b.Product)
                .WithMany()
                .HasForeignKey(b => b.ProductId);

            builder.HasOne(b => b.ImportInvoiceDetail)
                .WithMany(d => d.InventoryBatches)
                .HasForeignKey(b => b.ImportInvoiceDetailId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.Branch)
                .WithMany(branch => branch.InventoryBatches) 
                .HasForeignKey(b => b.BranchId);

        }
    }

}
