using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IQueryable<Order> GetAllForPaging();
        Task<Order?> GetOrderWithDetailsAsync(Guid id);
    
    }
}
