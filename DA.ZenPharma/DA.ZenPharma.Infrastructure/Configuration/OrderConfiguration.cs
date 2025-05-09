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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.HandledBy)
                   .WithMany()
                   .HasForeignKey(o => o.HandledById)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Branch)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BranchId);
        }
    }


}
