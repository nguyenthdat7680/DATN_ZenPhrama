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
    public class InventoryBatchRepository : GenericRepository<InventoryBatch>, IInventoryBatchRepository
    {
        public InventoryBatchRepository(ZenPharmaDbContext context) : base(context) { }

        public IQueryable<InventoryBatch> GetAllForPaging()
        {
            return _context.InventoryBatches.AsNoTracking();
        }
        public async Task<List<InventoryBatch>> GetByProductIdAsync(Guid productId)
        {
            return await _context.InventoryBatches
                .AsNoTracking()
                .Where(b => b.ProductId == productId)
                .Include(b => b.Product)
                .ToListAsync();
        }
    }
}
