using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductUnits);
        }

        public async Task<List<Product>> SearchByNameAsync(string keyword, int take = 5)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductUnits)
                .Where(p => p.ProductName.Contains(keyword))
                .OrderBy(p => p.ProductName)
                .Take(take)
                .ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products.AnyAsync(predicate);
        }

        public async Task<(List<Product> Items, int TotalItems)> SearchAsync(
            string? keyword, Guid? categoryId, string? orderBy, int page, int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductUnits)
                .AsQueryable();

            // Lọc theo từ khóa
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(keyword) ||
                                        p.ProductCode.ToLower().Contains(keyword) ||
                                        p.SKU.ToLower().Contains(keyword));
            }

            // Lọc theo danh mục
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // Sắp xếp
            query = orderBy?.ToLower() switch
            {
                "name" => query.OrderBy(p => p.ProductName),
                "price" => query.OrderBy(p => p.RegularPrice),
                _ => query.OrderByDescending(p => p.CreateDate)
            };

            // Tính tổng số mục
            var totalItems = await query.CountAsync();

            // Phân trang
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
    }
}
