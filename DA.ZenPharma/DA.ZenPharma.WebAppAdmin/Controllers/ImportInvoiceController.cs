using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDtos;
using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Dtos.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class ImportInvoiceController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/ImportInvoice";
        private const string ProductApiUrl = "https://localhost:7034/api/Product";
        private const string CategoryApiUrl = "https://localhost:7034/api/Category";

        public ImportInvoiceController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<ImportInvoiceDto>>(
                $"{ApiUrl}/paged?page={page}&pageSize={pageSize}");

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
            var invoice = await _httpClient.GetFromJsonAsync<ImportInvoiceDto>($"{ApiUrl}/{id}");

            if (invoice == null)
            {
                return NotFound("Hóa đơn không tồn tại.");
            }

            return View(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var staffIdString = HttpContext.Session.GetString("userId");
            if (!Guid.TryParse(staffIdString, out var staffId))
            {
                return RedirectToAction("Login", "Account");
            }

            var staff = await _httpClient.GetFromJsonAsync<UserDto>($"https://localhost:7034/api/User/{staffId}");
            ViewBag.StaffName = staff?.LastName ?? "Không xác định";

            if (User.IsInRole("Admin"))
            {
                var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                ViewBag.Branches = new SelectList(branches, "Id", "BranchName");
            }
            else
            {
                ViewBag.BranchName = HttpContext.Session.GetString("branchName");
                ViewBag.BranchId = HttpContext.Session.GetString("branchId");
            }
            ViewBag.UnitSuggestions = await _httpClient.GetFromJsonAsync<List<string>>($"{ProductApiUrl}/unit-suggestions") ?? new List<string>();
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(CategoryApiUrl);
            ViewBag.Categories = categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList() ?? new List<SelectListItem>();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ImportInvoiceCreateDto dto)
        {
            var staffIdString = HttpContext.Session.GetString("userId");
            if (!Guid.TryParse(staffIdString, out var staffId))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                

                var staff = await _httpClient.GetFromJsonAsync<UserDto>($"https://localhost:7034/api/User/{staffId}");
                ViewBag.StaffName = staff?.LastName ?? "Không xác định";

                if (User.IsInRole("Admin"))
                {
                    var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                    ViewBag.Branches = new SelectList(branches, "Id", "BranchName");
                }
                else
                {
                    ViewBag.BranchName = HttpContext.Session.GetString("branchName");
                    ViewBag.BranchId = HttpContext.Session.GetString("branchId");
                }


                return View(dto);
            }

            dto.HandledByStaffId = staffId;

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Hóa đơn đã được thêm thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi thêm hóa đơn!";
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var invoice = await _httpClient.GetFromJsonAsync<ImportInvoiceDto>($"{ApiUrl}/{id}");

            if (invoice == null)
            {
                return NotFound("Hóa đơn không tồn tại.");
            }

            var dto = new ImportInvoiceUpdateDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                BranchLocationId = invoice.BranchLocationId,
                SupplierId = invoice.SupplierId,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                HandledByStaffId = invoice.HandledByStaffId
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImportInvoiceUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Hóa đơn đã được cập nhật thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi cập nhật hóa đơn!";
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Hóa đơn đã được xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi xóa hóa đơn!";
            }

            return RedirectToAction("Index");
        }


    }
}
