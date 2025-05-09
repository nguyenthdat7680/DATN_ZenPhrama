    using DA.ZenPharma.Application.Dtos.BaseDto;
    using DA.ZenPharma.Application.Dtos.BranchDto;
    using DA.ZenPharma.Application.Dtos.OrderDetailDtos;
    using DA.ZenPharma.Application.Dtos.OrderDto;
    using DA.ZenPharma.Application.Dtos.ProductDto;
    using DA.ZenPharma.Application.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    namespace DA.ZenPharma.WebAppAdmin.Controllers
    {
        public class OrderController : Controller
        {
            private readonly HttpClient _httpClient;
            private const string ApiUrl = "https://localhost:7034/api/Order";

            public OrderController(IHttpClientFactory httpClientFactory)
            {
                _httpClient = httpClientFactory.CreateClient();
            }

            public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? branchId = null, string? status = null)
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

                ViewBag.StatusFilter = status;

                string url = $"{ApiUrl}/paged?page={page}&pageSize={pageSize}";

                if (!string.IsNullOrEmpty(branchId))
                    url += $"&branchId={branchId}";
                if (!string.IsNullOrEmpty(status))
                    url += $"&status={status}";

                var response = await _httpClient.GetFromJsonAsync<PageResultDto<OrderDto>>(url);

                if (response == null)
                    return View("Error");

                ViewBag.TotalItems = response.TotalItems;
                ViewBag.PageSize = response.PageSize;
                ViewBag.TotalPages = response.TotalPages;
                ViewBag.Page = page;

                return View(response);
            }

            [Authorize]
            [HttpGet]
            public async Task<IActionResult> Create()
            {
                var model = new OrderCreateDto
                {
                    Details = new List<OrderDetailCreateDto>()
                };

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

                return View(model);
            }

            [Authorize]
            [HttpPost]
            public async Task<IActionResult> Create(OrderCreateDto dto)
            {
                if (!ModelState.IsValid)
                {
                    if (User.IsInRole("Admin"))
                    {
                        var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                        ViewBag.Branches = new SelectList(branches, "Id", "BranchName");
                    }
                    else
                    {
                        ViewBag.BranchName = HttpContext.Session.GetString("BranchName");
                        ViewBag.BranchId = HttpContext.Session.GetString("BranchId");
                    }

                    return View(dto);
                }

                if (Guid.TryParse(HttpContext.Session.GetString("userId"), out var staffId))
                {
                    dto.HandledById = staffId;
                }

                if (!User.IsInRole("Admin") &&
                    Guid.TryParse(HttpContext.Session.GetString("BranchId"), out var branchId))
                {
                    dto.BranchId = branchId;
                }

                dto.CreatedAt = DateTime.UtcNow;
                dto.UpdatedAt = DateTime.UtcNow;

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7034/api/Order", dto);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi tạo đơn hàng!";
                return View(dto);
            }

            public async Task<IActionResult> Details(Guid id)
            {
                var order = await _httpClient.GetFromJsonAsync<OrderDto>($"{ApiUrl}/{id}");

                if (order == null)
                    return NotFound("Đơn hàng không tồn tại.");

                return View(order);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(OrderUpdateDto dto)
            {
                if (!ModelState.IsValid)
                    return View(dto);

                dto.UpdatedAt = DateTime.UtcNow;

                var response = await _httpClient.PutAsJsonAsync(ApiUrl, dto);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi cập nhật đơn hàng!";
                return View(dto);
            }

            [HttpPost]
            public async Task<IActionResult> Delete(Guid id)
            {
                var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Đơn hàng đã được xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi xóa đơn hàng!";
                }
                return RedirectToAction("Index");
            }
        }
    }
