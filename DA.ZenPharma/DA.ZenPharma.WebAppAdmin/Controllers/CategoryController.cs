using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/Category";

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<CategoryDto>>($"{ApiUrl}/paged?page={page}&pageSize={pageSize}");

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
            var category = await _httpClient.GetFromJsonAsync<CategoryDto>($"{ApiUrl}/{id}");

            if (category == null)
            {
                return NotFound("Danh mục không tồn tại.");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Danh mục đã được thêm thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi thêm danh mục!";
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _httpClient.GetFromJsonAsync<CategoryUpdateDto>($"{ApiUrl}/{id}");
            if (category == null)
                return NotFound("Không tìm thấy danh mục.");
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi cập nhật danh mục!";
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Danh mục đã được xóa thành công!";
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
                    ? "Đã có lỗi xảy ra khi xóa danh mục!"
                    : errorMessage;
            }

            return RedirectToAction("Index");
        }

    }
}
