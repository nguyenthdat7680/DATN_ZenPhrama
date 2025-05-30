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
    public class ReportRepository : IReportRepository
    {
        private readonly ZenPharmaDbContext _context;

        public ReportRepository(ZenPharmaDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetHotProductsRawDataAsync(int days, int topN)
        {
            var fromDate = DateTime.Now.AddDays(-days);
            return await _context.OrderDetails
                .Where(od => od.Order.OrderDate >= fromDate)
                .OrderByDescending(od => od.Quantity)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<List<Order>> GetRevenueRawDataAsync(int days)
        {
            var fromDate = DateTime.Now.AddDays(-days);
            return await _context.Orders
                .Where(o => o.OrderDate >= fromDate)
                .ToListAsync();
        }

        public async Task<int> CountOrdersAsync(DateTime from, DateTime to, Guid? branchId = null)
        {
            var query = _context.Orders
                .Where(o => o.OrderDate >= from && o.OrderDate < to);

            if (branchId.HasValue)
            {
                query = query.Where(o => o.BranchId == branchId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<decimal> CalculateRevenueAsync(DateTime from, DateTime to, Guid? branchId = null)
        {
            var query = _context.Orders
                .Where(o => o.OrderDate >= from && o.OrderDate < to);

            if (branchId.HasValue)
            {
                query = query.Where(o => o.BranchId == branchId.Value);
            }

            return await query.SumAsync(o => o.TotalAmount);
        }

        public async Task<int> CountNewCustomersAsync(DateTime from, DateTime to, Guid? branchId = null)
        {
            var query = _context.Users
                .Where(u => u.CreateDate >= from && u.CreateDate < to && u.Role.RoleName == "Customer");

            if (branchId.HasValue)
            {
                query = query.Where(u => u.BranchId == branchId.Value); // nếu User có BranchId
            }

            return await query.CountAsync();
        }

        public async Task<int> CountGuestVisitsAsync(DateTime from, DateTime to, Guid? branchId = null)
        {
            var query = _context.Orders
                .Where(o => o.CustomerName == null && o.OrderDate >= from && o.OrderDate < to);

            if (branchId.HasValue)
            {
                query = query.Where(o => o.BranchId == branchId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<List<(DateTime Date, decimal Revenue)>> GetRevenueRawAsync(DateTime fromDate, DateTime toDate, Guid? branchId)
        {
            var query = _context.Orders
                .Where(o => o.OrderDate.Date >= fromDate && o.OrderDate.Date <= toDate);

            if (branchId.HasValue)
            {
                query = query.Where(o => o.BranchId == branchId.Value);
            }

            var result = await query
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(x => x.TotalAmount) })
                .ToListAsync();

            return result.Select(x => (x.Date, x.Revenue)).ToList();
        }
        public IQueryable<Order> GetRecentOrders(Guid? branchId, int days)
        {
            var fromDate = DateTime.Now.AddDays(-days); 

            return _context.Orders
                .Include(o => o.User)
                .Where(o =>
                    o.OrderDate >= fromDate &&
                    (!branchId.HasValue || o.BranchId == branchId.Value)
                )
                .OrderByDescending(o => o.OrderDate);
        }


        public IQueryable<OrderDetail> GetOrderDetailsWithOrderAndProduct()
        {
            return _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.Product)
                .AsQueryable();
        }


    }


}
