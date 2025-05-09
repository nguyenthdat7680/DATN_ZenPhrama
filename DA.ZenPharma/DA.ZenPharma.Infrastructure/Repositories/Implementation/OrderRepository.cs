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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ZenPharmaDbContext context) : base(context) { }
        public IQueryable<Order> GetAllForPaging()
        {
            return _context.Orders.AsNoTracking();
        }
        public async Task<Order?> GetOrderWithDetailsAsync(Guid id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Branch)
                .Include(o => o.HandledBy)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
