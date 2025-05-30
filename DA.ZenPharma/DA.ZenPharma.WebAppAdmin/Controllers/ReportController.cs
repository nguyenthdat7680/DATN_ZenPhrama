using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Dtos.ReportDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/Report";

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                ViewBag.Branches = new SelectList(branches, "Id", "BranchName");
            }
            else
            {
                ViewBag.BranchId = HttpContext.Session.GetString("branchId");
                ViewBag.BranchName = HttpContext.Session.GetString("branchName");
            } 
            return View();
        }


        public async Task<IActionResult> HotProducts(int days = 30, int topN = 5)
        {
            var url = $"{ApiUrl}/hot-products?days={days}&topN={topN}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Không thể lấy dữ liệu sản phẩm bán chạy.";
                return View(new List<HotProductDto>());
            }

            var data = await response.Content.ReadFromJsonAsync<List<HotProductDto>>();
            return View(data);
        }

        public async Task<IActionResult> Revenue(int days = 30)
        {
            var url = $"{ApiUrl}/revenue?days={days}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Không thể lấy dữ liệu doanh thu.";
                return View(new List<RevenueByDateDto>());
            }

            var data = await response.Content.ReadFromJsonAsync<List<RevenueByDateDto>>();
            return View(data);
        }
    }
}
