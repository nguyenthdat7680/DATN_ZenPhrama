using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.RoleDtos;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/Role";

        public RoleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<RoleDto>>($"{ApiUrl}/paged?page={page}&pageSize={pageSize}");
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
            var role = await _httpClient.GetFromJsonAsync<RoleDto>($"{ApiUrl}/{id}");
            if (role == null)
                return NotFound("Không tìm thấy vai trò.");
            return View(role);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm vai trò thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Lỗi khi thêm vai trò!";
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _httpClient.GetFromJsonAsync<RoleUpdateDto>($"{ApiUrl}/{id}");
            if (role == null)
                return NotFound("Không tìm thấy vai trò.");
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật vai trò thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Lỗi khi cập nhật vai trò!";
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Xoá vai trò thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Lỗi khi xoá vai trò!";
            }

            return RedirectToAction("Index");
        }
    }
}
