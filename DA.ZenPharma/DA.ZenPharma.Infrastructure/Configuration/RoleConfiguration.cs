using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.ZenPharma.Infrastructure.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.Property(x => x.RoleName)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.RoleName).IsUnique();

            builder.Property(x => x.Description).HasColumnType("NVARCHAR(MAX)");

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
