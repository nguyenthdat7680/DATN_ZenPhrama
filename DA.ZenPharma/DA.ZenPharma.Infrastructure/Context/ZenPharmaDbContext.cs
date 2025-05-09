using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace DA.ZenPharma.Infrastructure.Context
{
    public class ZenPharmaDbContext : DbContext
    {
        public ZenPharmaDbContext(DbContextOptions<ZenPharmaDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Prescription> Prescriptions { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public DbSet<ImportInvoice> ImportInvoices { get; set; }
        public DbSet<ImportInvoiceDetail> ImportInvoiceDetails { get; set; }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Commune> Communes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<InventoryBatch> InventoryBatches { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZenPharmaDbContext).Assembly);
        }
    }
}
