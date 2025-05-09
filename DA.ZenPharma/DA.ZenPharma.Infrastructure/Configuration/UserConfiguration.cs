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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(x => x.FirstName).HasMaxLength(255);
            builder.Property(x => x.LastName).HasMaxLength(255);
            builder.Property(x => x.PhoneNumber).HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(255);
            builder.Property(x => x.AvatarImagePath).HasMaxLength(255);
            builder.Property(x => x.PasswordHash).HasMaxLength(255);
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Branch)
                .WithMany(r => r.Users) 
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
