using DA.ZenPharma.Application.Dtos.SupplierDto;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DA.ZenPharma.Application.Dtos.BaseDto;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "https://localhost:7034/api/Supplier";

        public SupplierController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<PageResultDto<SupplierDto>>($"{ApiUrl}/paged?page={page}&pageSize={pageSize}");

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

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SupplierCreateDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Lỗi khi thêm nhà cung cấp.";
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var supplier = await client.GetFromJsonAsync<SupplierUpdateDto>($"{ApiUrl}/{id}");

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierUpdateDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Lỗi khi cập nhật.";
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{ApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Đã xoá nhà cung cấp.";
            else
                TempData["ErrorMessage"] = "Không thể xoá nhà cung cấp.";

            return RedirectToAction("Index");
        }
    }
}
