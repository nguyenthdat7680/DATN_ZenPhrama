using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.ReportDtos;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<HotProductDto>> GetHotProductsAsync(int days, int topN);
        Task<List<RevenueByDateDto>> GetRevenueByDateAsync(int days);
        Task<ReportSummaryDto> GetSummaryAsync(int days, Guid? branchId = null);

        Task<List<RevenueChartDto>> GetRevenueChartAsync(int days, Guid? branchId);

        Task<List<RecentOrderDto>> GetRecentOrdersAsync(int days, Guid? branchId, int take = 5);
        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int days, Guid? branchId, int take = 5);
    }

}
