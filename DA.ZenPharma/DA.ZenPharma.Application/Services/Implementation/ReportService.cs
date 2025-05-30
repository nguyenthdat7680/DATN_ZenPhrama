using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.ReportDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<HotProductDto>> GetHotProductsAsync(int days, int topN)
        {
            var rawData = await _unitOfWork.Reports.GetHotProductsRawDataAsync(days, topN);

            var hotProducts = rawData
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new HotProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(topN)
                .ToList();

            return hotProducts;
        }

        public async Task<List<RevenueByDateDto>> GetRevenueByDateAsync(int days)
        {
            var rawData = await _unitOfWork.Reports.GetRevenueRawDataAsync(days);

            var revenueByDate = rawData
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new RevenueByDateDto
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return revenueByDate;
        }

        public async Task<ReportSummaryDto> GetSummaryAsync(int days, Guid? branchId = null)
        {
            var toDate = DateTime.Now;
            var fromDate = toDate.AddDays(-days);
            var previousToDate = fromDate;
            var previousFromDate = fromDate.AddDays(-days);

            var currentOrders = await _unitOfWork.Reports.CountOrdersAsync(fromDate, toDate, branchId);
            var currentRevenue = await _unitOfWork.Reports.CalculateRevenueAsync(fromDate, toDate, branchId);
            var currentCustomers = await _unitOfWork.Reports.CountNewCustomersAsync(fromDate, toDate, branchId);
            var currentVisitors = await _unitOfWork.Reports.CountGuestVisitsAsync(fromDate, toDate, branchId);

            var previousOrders = await _unitOfWork.Reports.CountOrdersAsync(previousFromDate, previousToDate, branchId);
            var previousRevenue = await _unitOfWork.Reports.CalculateRevenueAsync(previousFromDate, previousToDate, branchId);
            var previousCustomers = await _unitOfWork.Reports.CountNewCustomersAsync(previousFromDate, previousToDate, branchId);
            var previousVisitors = await _unitOfWork.Reports.CountGuestVisitsAsync(previousFromDate, previousToDate, branchId);

            double CalculateChange(double current, double previous)
                => previous == 0 ? 100 : ((current - previous) / previous) * 100;

            return new ReportSummaryDto
            {
                TotalOrders = currentOrders,
                TotalRevenue = currentRevenue,
                NewCustomers = currentCustomers,
                GuestVisits = currentVisitors,
                OrderChangeRate = CalculateChange(currentOrders, previousOrders),
                RevenueChangeRate = CalculateChange((double)currentRevenue, (double)previousRevenue),
                CustomerChangeRate = CalculateChange(currentCustomers, previousCustomers),
                VisitorChangeRate = CalculateChange(currentVisitors, previousVisitors)
            };
        }

        public async Task<List<RevenueChartDto>> GetRevenueChartAsync(int days, Guid? branchId)
        {
            var toDate = DateTime.UtcNow.Date;
            var fromDate = toDate.AddDays(-days + 1);

            var rawData = await _unitOfWork.Reports.GetRevenueRawAsync(fromDate, toDate, branchId);

            var result = new List<RevenueChartDto>();
            for (int i = 0; i < days; i++)
            {
                var date = fromDate.AddDays(i);
                var matched = rawData.FirstOrDefault(x => x.Date == date);

                result.Add(new RevenueChartDto
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Revenue = matched.Revenue
                });
            }

            return result;
        }

        public async Task<List<RecentOrderDto>> GetRecentOrdersAsync(int days, Guid? branchId, int take = 5)
        {
            var query = _unitOfWork.Reports.GetRecentOrders(branchId, days)
                .Take(take)
                .Select(o => new RecentOrderDto
                {
                    Code = o.OrderCode,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.OrderStatus
                });

            return await query.ToListAsync();
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int days, Guid? branchId, int take = 5)
        {
            var fromDate = DateTime.Now.AddDays(-days); 

            var query = _unitOfWork.Reports.GetOrderDetailsWithOrderAndProduct();

            query = query.Where(x => x.Order.OrderDate >= fromDate);

            if (branchId.HasValue)
                query = query.Where(x => x.Order.BranchId == branchId.Value);

            var grouped = await query
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.ProductCode,
                    x.Product.ProductName,
                    x.Product.RegularPrice,
                    x.Product.ThumbnailImagePath
                })
                .Select(g => new TopSellingProductDto
                {
                    ProductCode = g.Key.ProductCode,
                    ProductName = g.Key.ProductName,
                    Price = g.Key.RegularPrice,
                    ImageUrl = g.Key.ThumbnailImagePath,
                    TotalQuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(take)
                .ToListAsync();

            return grouped;
        }



    }

}
