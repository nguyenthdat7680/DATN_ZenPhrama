using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.Context;
using DA.ZenPharma.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Infrastructure.Repositories.Implementation
{
    public class ImportInvoiceRepository : GenericRepository<ImportInvoice>, IImportInvoiceRepository
    {
        public ImportInvoiceRepository(ZenPharmaDbContext context) : base(context) { }

        public IQueryable<ImportInvoice> GetAllForPaging()
        {
            return _context.ImportInvoices;
        }
        public async Task<ImportInvoice?> GetById(Guid id)
        {
            return await _context.ImportInvoices
                .Include(i => i.Branch)
                .Include(i => i.Supplier)
                .Include(i => i.Staff)
                .Include(i => i.Details)
                    .ThenInclude(d => d.Product)
                .Include(i => i.Details)
                    .ThenInclude(d => d.InventoryBatches)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

    }
}
