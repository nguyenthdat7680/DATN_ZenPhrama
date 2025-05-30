using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("hot-products")]
        public async Task<IActionResult> GetHotProducts([FromQuery] int days = 30, [FromQuery] int topN = 5)
        {
            var result = await _reportService.GetHotProductsAsync(days, topN);
            return Ok(result);
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue([FromQuery] int days = 30)
        {
            var result = await _reportService.GetRevenueByDateAsync(days);
            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(int days, Guid? branchId = null)
        {
            if (days <= 0) return BadRequest("Số ngày phải lớn hơn 0.");

            var summary = await _reportService.GetSummaryAsync(days, branchId);
            return Ok(summary);
        }

        [HttpGet("revenue-chart")]
        public async Task<IActionResult> GetRevenueChart([FromQuery] int days, [FromQuery] Guid? branchId)
        {
            var data = await _reportService.GetRevenueChartAsync(days, branchId);
            return Ok(data);
        }

        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders([FromQuery] int days, Guid? branchId, [FromQuery] int take = 5)
        {
            var result = await _reportService.GetRecentOrdersAsync(days, branchId, take);
            return Ok(result);
        }

        [HttpGet("top-selling-products")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int days, [FromQuery] Guid? branchId, [FromQuery] int take = 5)
        {
            var result = await _reportService.GetTopSellingProductsAsync(days, branchId, take);
            return Ok(result);
        }
    }
}
