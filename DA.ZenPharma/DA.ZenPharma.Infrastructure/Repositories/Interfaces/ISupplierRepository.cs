using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Infrastructure.Repositories.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        IQueryable<Supplier> GetAllForPaging();
        Task<List<Supplier>> SearchByNameAsync(string keyword, int take = 10);

    }
}
