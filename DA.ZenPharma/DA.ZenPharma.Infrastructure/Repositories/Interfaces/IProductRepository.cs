using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetAllForPaging();
        Task<List<Product>> SearchByNameAsync(string keyword, int take = 5);
        Task<bool> AnyAsync(Expression<Func<Product, bool>> predicate);
        Task<(List<Product> Items, int TotalItems)> SearchAsync(
            string? keyword, Guid? categoryId, string? orderBy, int page, int pageSize);
    }
}
