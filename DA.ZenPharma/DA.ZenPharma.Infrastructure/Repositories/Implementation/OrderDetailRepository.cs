using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.Context;
using DA.ZenPharma.Infrastructure.Repositories.Interfaces;

namespace DA.ZenPharma.Infrastructure.Repositories.Implementation
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ZenPharmaDbContext context) : base(context) { }
    }
}
