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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branch");

            builder.Property(b => b.BranchName).IsRequired().HasMaxLength(255);
            builder.Property(b => b.BranchCode).HasMaxLength(50);
            builder.Property(b => b.Address).HasMaxLength(255);
            builder.Property(b => b.PhoneNumber).HasMaxLength(50);
            builder.Property(b => b.Email).HasMaxLength(255);
            builder.Property(b => b.IsActive).HasDefaultValue(true);

            
        }
    }

}
