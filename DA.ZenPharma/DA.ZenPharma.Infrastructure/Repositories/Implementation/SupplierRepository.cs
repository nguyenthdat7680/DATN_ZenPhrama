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
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ZenPharmaDbContext context) : base(context) { }
        public IQueryable<Supplier> GetAllForPaging()
        {
            return _context.Suppliers;
        }
        public async Task<List<Supplier>> SearchByNameAsync(string? keyword, int take = 10)
        {
            return await _context.Suppliers
                .Where(s => string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword))
                .OrderBy(s => s.Name)
                .Take(take)
                .ToListAsync();
        }



    }
}
