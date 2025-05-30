using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.Context;
using DA.ZenPharma.Infrastructure.Repositories;
using DA.ZenPharma.Infrastructure.Repositories.Implementation;
using DA.ZenPharma.Infrastructure.Repositories.Interfaces;

namespace DA.ZenPharma.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ZenPharmaDbContext _context;

        public UnitOfWork(ZenPharmaDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Roles = new RoleRepository(_context);
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Suppliers = new SupplierRepository(_context);
            Branches = new BranchRepository(_context);
            Carts = new CartRepository(_context);
            CartItems = new CartItemRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            Prescriptions = new PrescriptionRepository(_context);
            ImportInvoices = new ImportInvoiceRepository(_context);
            ImportInvoiceDetails = new ImportInvoiceDetailRepository(_context);
            Addresses = new AddressRepository(_context);
            InventoryBatches = new InventoryBatchRepository(_context);
            Reports = new ReportRepository(_context);
            ProductUnits = new GenericRepository<ProductUnit>(_context);
        }

        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public ISupplierRepository Suppliers { get; }
        public IBranchRepository Branches { get; }
        public ICartRepository Carts { get; }
        public ICartItemRepository CartItems { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IPrescriptionRepository Prescriptions { get; }
        public IImportInvoiceRepository ImportInvoices { get; }
        public IImportInvoiceDetailRepository ImportInvoiceDetails { get; }
        public IAddressRepository Addresses { get; }
        public IInventoryBatchRepository InventoryBatches { get; }
        public IReportRepository Reports { get; }
        public IGenericRepository<ProductUnit> ProductUnits { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
