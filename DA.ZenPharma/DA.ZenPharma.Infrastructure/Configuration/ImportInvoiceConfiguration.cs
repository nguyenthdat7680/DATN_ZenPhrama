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
    public class ImportInvoiceConfiguration : IEntityTypeConfiguration<ImportInvoice>
    {
        public void Configure(EntityTypeBuilder<ImportInvoice> builder)
        {
            builder.ToTable("ImportInvoice");

            builder.Property(i => i.InvoiceNumber).HasMaxLength(50);
            builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Branch)
                .WithMany(b => b.ImportInvoices)
                .HasForeignKey(i => i.BranchLocationId);

            builder.HasOne(i => i.Supplier)
                .WithMany()
                .HasForeignKey(i => i.SupplierId);

            builder.HasOne(i => i.Staff)
                .WithMany()
                .HasForeignKey(i => i.HandledByStaffId);
        }
    }

}
