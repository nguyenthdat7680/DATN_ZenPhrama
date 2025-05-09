using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class InventoryBatchController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/InventoryBatch";

        public InventoryBatchController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? branchId = null, string? productName = null, string? batchName = null)
        {
            if (User.IsInRole("Admin"))
            {
                var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                ViewBag.Branches = new SelectList(branches, "Id", "BranchName");
                ViewBag.BranchFilter = branchId;
            }
            else
            {
                branchId = HttpContext.Session.GetString("branchId");
                ViewBag.BranchId = branchId;
                ViewBag.BranchName = HttpContext.Session.GetString("branchName");
            }

            ViewBag.ProductName = productName;
            ViewBag.BatchName = batchName;

            string url = $"https://localhost:7034/api/InventoryBatch/paged?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(branchId))
                url += $"&branchId={branchId}";
            if (!string.IsNullOrEmpty(productName))
                url += $"&productName={productName}";
            if (!string.IsNullOrEmpty(batchName))
                url += $"&batchName={batchName}";

            var result = await _httpClient.GetFromJsonAsync<PageResultDto<InventoryBatchDto>>(url);

            if (result == null)
                return View("Error");

            ViewBag.TotalItems = result.TotalItems;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.Page = page;

            return View(result);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _httpClient.GetFromJsonAsync<InventoryBatchDto>($"{ApiUrl}/{id}");
            if (item == null) return NotFound("Lô hàng không tồn tại.");
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryBatchCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return View(createDto);

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, createDto);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm lô hàng thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Lỗi khi thêm lô hàng!";
            return View(createDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _httpClient.GetFromJsonAsync<InventoryBatchDto>($"{ApiUrl}/{id}");
            if (item == null) return NotFound("Lô hàng không tồn tại.");
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InventoryBatchUpdateDto updateDto)
        {
            if (!ModelState.IsValid) return View(updateDto);

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, updateDto);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Lỗi khi cập nhật lô hàng!";
            return View(updateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");

            TempData["SuccessMessage"] = response.IsSuccessStatusCode
                ? "Xoá lô hàng thành công!"
                : "Lỗi khi xoá lô hàng!";

            return RedirectToAction("Index");
        }
    }
}
