using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetAllForPaging();
        Task<List<Product>> SearchByNameAsync(string keyword, int take = 5);
    }
}
