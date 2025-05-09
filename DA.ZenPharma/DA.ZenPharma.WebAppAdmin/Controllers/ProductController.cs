using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/Product";

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>($"{ApiUrl}/paged?page={page}&pageSize={pageSize}");

            if (response == null)
            {
                return View("Error");
            }

            ViewBag.TotalItems = response.TotalItems;
            ViewBag.PageSize = response.PageSize;
            ViewBag.TotalPages = response.TotalPages;
            ViewBag.Page = page;

            return View(response);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"{ApiUrl}/{id}");

            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("https://localhost:7034/api/Category");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("https://localhost:7034/api/Category");
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                return View(productCreateDto);
            }

            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}", productCreateDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi thêm sản phẩm!";
            return View(productCreateDto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"{ApiUrl}/{id}");

            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateDto productUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productUpdateDto);
            }

            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}", productUpdateDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sản phẩm đã được cập nhật thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi cập nhật sản phẩm!";
            return View(productUpdateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi xóa sản phẩm!";
            }

            return RedirectToAction("Index");
        }
    }
}
