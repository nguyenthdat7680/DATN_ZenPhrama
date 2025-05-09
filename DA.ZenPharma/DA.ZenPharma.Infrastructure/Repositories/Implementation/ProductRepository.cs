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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ZenPharmaDbContext context) : base(context) { }

        public IQueryable<Product> GetAllForPaging()
        {
            return _context.Products;
        }
        public async Task<List<Product>> SearchByNameAsync(string keyword, int take = 5)
        {
            return await _context.Products
                .Where(p => p.ProductName.Contains(keyword))
                .OrderBy(p => p.ProductName)
                .Take(take)
                .ToListAsync();
        }
    }
}
