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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItem");

            builder.HasOne(i => i.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CartId);

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
        }
    }

}
