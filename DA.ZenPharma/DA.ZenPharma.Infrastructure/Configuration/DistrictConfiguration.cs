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
    public class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("District");

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(d => d.Province)
                .WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId);
        }
    }

}
