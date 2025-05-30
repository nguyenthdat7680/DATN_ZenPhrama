using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Infrastructure.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<OrderDetail>> GetHotProductsRawDataAsync(int days, int topN);
        Task<List<Order>> GetRevenueRawDataAsync(int days);

        Task<int> CountOrdersAsync(DateTime from, DateTime to, Guid? branchId = null);
        Task<decimal> CalculateRevenueAsync(DateTime from, DateTime to, Guid? branchId = null);
        Task<int> CountNewCustomersAsync(DateTime from, DateTime to, Guid? branchId = null);
        Task<int> CountGuestVisitsAsync(DateTime from, DateTime to, Guid? branchId = null);

        Task<List<(DateTime Date, decimal Revenue)>> GetRevenueRawAsync(DateTime fromDate, DateTime toDate, Guid? branchId);

        IQueryable<Order> GetRecentOrders(Guid? branchId, int days);
        IQueryable<OrderDetail> GetOrderDetailsWithOrderAndProduct();
    }
}
