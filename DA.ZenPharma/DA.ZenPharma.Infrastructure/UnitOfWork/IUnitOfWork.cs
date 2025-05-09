using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Infrastructure.Repositories.Interfaces;

namespace DA.ZenPharma.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ISupplierRepository Suppliers { get; }
        IBranchRepository Branches { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IPrescriptionRepository Prescriptions { get; }
        IImportInvoiceRepository ImportInvoices { get; }
        IImportInvoiceDetailRepository ImportInvoiceDetails { get; }
        IAddressRepository Addresses { get; }
        IInventoryBatchRepository InventoryBatches { get; }

        Task<int> SaveChangesAsync();
    }
}
