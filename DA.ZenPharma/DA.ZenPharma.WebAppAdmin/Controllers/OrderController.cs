    using DA.ZenPharma.Application.Dtos.BaseDto;
    using DA.ZenPharma.Application.Dtos.BranchDto;
    using DA.ZenPharma.Application.Dtos.OrderDetailDtos;
    using DA.ZenPharma.Application.Dtos.OrderDto;
    using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Dtos.ProductUnitDtos;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Fetch the order
            var response = await _httpClient.GetAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Không thể tìm thấy đơn hàng.";
                return RedirectToAction("Index");
            }

            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            if (order == null || order.OrderStatus != "Draft")
            {
                TempData["ErrorMessage"] = "Đơn hàng không tồn tại hoặc không phải trạng thái tạm.";
                return RedirectToAction("Index");
            }

            // Map to OrderCreateDto
            var model = new OrderCreateDto
            {
                Id = order.Id,
                BranchId = order.BranchId,
                CustomerName = order.CustomerName ?? "Khách lẻ",
                CustomerPhone = order.CustomerPhone,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                HandledById = order.UserId ?? Guid.Empty,
                UpdatedAt = DateTime.UtcNow,
                Details = order.OrderDetails?.Select(d => new OrderDetailCreateDto
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    Unit = d.Unit ?? "Viên",
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    InventoryBatchId = d.InventoryBatchId ?? Guid.Empty
                }).ToList() ?? new List<OrderDetailCreateDto>()
            };
            Console.WriteLine($"Số chi tiết đơn hàng: {order.OrderDetails?.Count ?? 0}");
            Console.WriteLine($"Model.Details: {System.Text.Json.JsonSerializer.Serialize(model.Details)}");
            // Fetch product names and units for display
            var productNames = new Dictionary<Guid, string>();
            var productUnits = new Dictionary<Guid, List<ProductUnitDto>>();
            foreach (var detail in order.OrderDetails ?? new List<OrderDetailDto>())
            {
                try
                {
                    // Fetch product details
                    var productResponse = await _httpClient.GetAsync($"https://localhost:7034/api/Product/{detail.ProductId}");
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var product = await productResponse.Content.ReadFromJsonAsync<ProductDto>();
                        productNames[detail.ProductId] = product?.ProductName ?? "Không rõ";
                    }
                    else
                    {
                        productNames[detail.ProductId] = "Không rõ";
                    }

                    // Fetch product units
                    var unitsResponse = await _httpClient.GetAsync($"https://localhost:7034/api/Product/{detail.ProductId}/units");
                    if (unitsResponse.IsSuccessStatusCode)
                    {
                        var units = await unitsResponse.Content.ReadFromJsonAsync<List<ProductUnitDto>>();
                        productUnits[detail.ProductId] = units ?? new List<ProductUnitDto>();
                    }
                    else
                    {
                        productUnits[detail.ProductId] = new List<ProductUnitDto>();
                    }
                }
                catch
                {
                    productNames[detail.ProductId] = "Không rõ";
                    productUnits[detail.ProductId] = new List<ProductUnitDto>();
                }
            }
            ViewBag.ProductNames = productNames;
            ViewBag.ProductUnits = productUnits;

            // Fetch branches for Admin
            if (User.IsInRole("Admin"))
            {
                try
                {
                    var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                    ViewBag.Branches = new SelectList(branches, "Id", "BranchName", model.BranchId);
                }
                catch
                {
                    ViewBag.Branches = new SelectList(new List<BranchDto>(), "Id", "BranchName");
                    TempData["ErrorMessage"] = "Không thể tải danh sách chi nhánh.";
                }
            }
            else
            {
                ViewBag.BranchName = HttpContext.Session.GetString("branchName") ?? "Không xác định";
                ViewBag.BranchId = HttpContext.Session.GetString("branchId");
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(OrderCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                    ViewBag.Branches = new SelectList(branches, "Id", "BranchName", dto.BranchId);
                }
                else
                {
                    ViewBag.BranchName = HttpContext.Session.GetString("branchName");
                    ViewBag.BranchId = HttpContext.Session.GetString("branchId");
                }
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View(dto);
            }

            // Set staff and branch info
            if (Guid.TryParse(HttpContext.Session.GetString("userId"), out var staffId))
                dto.HandledById = staffId;

            if (!User.IsInRole("Admin") && Guid.TryParse(HttpContext.Session.GetString("branchId"), out var branchId))
                dto.BranchId = branchId;

            dto.CreatedAt = DateTime.UtcNow;
            dto.UpdatedAt = DateTime.UtcNow;

            try
            {
                // Delete existing order
                if (dto.Id.HasValue)
                {
                    var deleteResponse = await _httpClient.DeleteAsync($"{ApiUrl}/{dto.Id}");
                    if (!deleteResponse.IsSuccessStatusCode)
                    {
                        TempData["ErrorMessage"] = "Không thể xóa đơn hàng cũ.";
                        return View(dto);
                    }
                }

                // Create new order
                var createResponse = await _httpClient.PostAsJsonAsync(ApiUrl, dto);
                if (createResponse.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                TempData["ErrorMessage"] = "Lỗi khi tạo đơn hàng mới.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
            }

            // Reload data if error occurs
            if (User.IsInRole("Admin"))
            {
                var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                ViewBag.Branches = new SelectList(branches, "Id", "BranchName", dto.BranchId);
            }
            else
            {
                ViewBag.BranchName = HttpContext.Session.GetString("branchName");
                ViewBag.BranchId = HttpContext.Session.GetString("branchId");
            }
            return View(dto);
        }

    }
    }
