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
    public class CommuneConfiguration : IEntityTypeConfiguration<Commune>
    {
        public void Configure(EntityTypeBuilder<Commune> builder)
        {
            builder.ToTable("Commune");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(c => c.District)
                .WithMany(d => d.Communes)
                .HasForeignKey(c => c.DistrictId);
        }
    }

}
