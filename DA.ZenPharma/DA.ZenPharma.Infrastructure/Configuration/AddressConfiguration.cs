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
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.Property(a => a.AddressLine1)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.AddressLine2)
                .HasMaxLength(255);

            builder.Property(a => a.ReceiverName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.ReceiverPhone)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(a => a.User)
                .WithMany(u => u.Addresses)  
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Commune)
                .WithMany()
                .HasForeignKey(a => a.CommuneId);
        }
    }

}
