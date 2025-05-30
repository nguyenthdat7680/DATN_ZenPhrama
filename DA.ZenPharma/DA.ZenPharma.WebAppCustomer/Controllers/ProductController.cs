using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAppCustomer.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/Product";

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? keyword, string? orderBy, Guid? categoryId, int pageIndex = 1, int pageSize = 8)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 8;

            var queryString = $"?page={pageIndex}&pageSize={pageSize}" +
                             (string.IsNullOrEmpty(keyword) ? "" : $"&keyword={Uri.EscapeDataString(keyword.Trim())}") +
                             (string.IsNullOrEmpty(orderBy) ? "" : $"&orderBy={orderBy}") +
                             (categoryId.HasValue ? $"&categoryId={categoryId}" : "");

            try
            {
                var response = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>($"{ApiUrl}/search{queryString}");

                if (response == null || !response.Items.Any())
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm phù hợp.";
                }

                ViewBag.TotalRecords = response.TotalItems;
                ViewBag.PageSize = response.PageSize;
                ViewBag.PageCount = response.TotalPages;
                ViewBag.PageIndex = response.Page;
                ViewBag.Keyword = keyword;
                ViewBag.OrderBy = orderBy;
                ViewBag.CategoryId = categoryId;

                return View(response);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tìm kiếm sản phẩm. Vui lòng thử lại sau.";
                return View(new PageResultDto<ProductDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var product = await _httpClient.GetFromJsonAsync<ProductDto>($"{ApiUrl}/{id}");

                if (product == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi lấy chi tiết sản phẩm.";
                return View();
            }
        }
    }
}
