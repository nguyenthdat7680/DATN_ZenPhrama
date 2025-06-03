using System.Net.Http.Headers;
using System.Text.Json;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Dtos.ProductUnitDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/Product";
        private const string CategoryApiUrl = "https://localhost:7034/api/Category";

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(
            [FromQuery] string? keyword = null,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] string? orderBy = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page và pageSize phải lớn hơn 0.");

            // Construct the API URL with query parameters
            var query = new List<string>();
            if (!string.IsNullOrEmpty(keyword)) query.Add($"keyword={Uri.EscapeDataString(keyword)}");
            if (categoryId.HasValue) query.Add($"categoryId={categoryId.Value}");
            if (!string.IsNullOrEmpty(orderBy)) query.Add($"orderBy={orderBy}");
            query.Add($"page={page}");
            query.Add($"pageSize={pageSize}");

            var apiUrl = $"{ApiUrl}/search{(query.Any() ? "?" + string.Join("&", query) : "")}";
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>(apiUrl);
            if (response == null) return View("Error");

            // Fetch categories for the dropdown
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(CategoryApiUrl);
            ViewBag.Categories = categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList() ?? new List<SelectListItem>();

            // Fetch unit suggestions
            ViewBag.UnitSuggestions = await _httpClient.GetFromJsonAsync<List<string>>($"{ApiUrl}/unit-suggestions") ?? new List<string>();

            // Pass search parameters to ViewBag for form persistence
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId?.ToString();
            ViewBag.OrderBy = orderBy;
            ViewBag.TotalItems = response.TotalItems;
            ViewBag.PageSize = response.PageSize;
            ViewBag.TotalPages = response.TotalPages;
            ViewBag.Page = page;

            return View(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? keyword,
            [FromQuery] Guid? categoryId,
            [FromQuery] string? orderBy,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page và pageSize phải lớn hơn 0.");

            // Construct the API URL with query parameters
            var query = new List<string>();
            if (!string.IsNullOrEmpty(keyword)) query.Add($"keyword={Uri.EscapeDataString(keyword)}");
            if (categoryId.HasValue) query.Add($"categoryId={categoryId.Value}");
            if (!string.IsNullOrEmpty(orderBy)) query.Add($"orderBy={orderBy}");
            query.Add($"page={page}");
            query.Add($"pageSize={pageSize}");

            var apiUrl = $"{ApiUrl}/search{(query.Any() ? "?" + string.Join("&", query) : "")}";
            var result = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>(apiUrl);
            if (result == null) return View("Error");

            // Fetch categories for the dropdown
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(CategoryApiUrl);
            ViewBag.Categories = categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList() ?? new List<SelectListItem>();

            // Fetch unit suggestions
            ViewBag.UnitSuggestions = await _httpClient.GetFromJsonAsync<List<string>>($"{ApiUrl}/unit-suggestions") ?? new List<string>();

            // Pass search parameters to ViewBag for form persistence
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId?.ToString();
            ViewBag.OrderBy = orderBy;
            ViewBag.TotalItems = result.TotalItems;
            ViewBag.PageSize = result.PageSize;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.Page = page;

            return View("Index", result);
        }
        [HttpPost]
        [Route("Product/UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile ThumbnailImage)
        {
            if (ThumbnailImage == null || ThumbnailImage.Length == 0)
            {
                return BadRequest(new { success = false, message = "Không có ảnh được chọn." });
            }

            try
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ThumbnailImage.FileName);
                var filePath = Path.Combine("wwwroot/images/products", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ThumbnailImage.CopyToAsync(stream);
                }

                var imagePath = $"/images/products/{fileName}";
                return Ok(new { success = true, thumbnailImagePath = imagePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi khi lưu ảnh: {ex.Message}" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto, IFormFile? ThumbnailImage)
        {
            try
            {
                var formData = new MultipartFormDataContent();

                // Log dữ liệu
                Console.WriteLine("ProductCreateDto:");
                foreach (var property in typeof(ProductCreateDto).GetProperties())
                {
                    var value = property.GetValue(dto);
                    Console.WriteLine($"{property.Name}: {value}");
                    if (value != null && property.Name != "ProductUnits")
                        formData.Add(new StringContent(value.ToString()), property.Name);
                }

                if (dto.ProductUnits != null)
                {
                    Console.WriteLine("ProductUnits:");
                    for (int i = 0; i < dto.ProductUnits.Count; i++)
                    {
                        Console.WriteLine($"Unit[{i}]: {dto.ProductUnits[i].Unit}, ConversionFactor: {dto.ProductUnits[i].ConversionFactor}");
                        if (!string.IsNullOrEmpty(dto.ProductUnits[i].Unit))
                            formData.Add(new StringContent(dto.ProductUnits[i].Unit), $"ProductUnits[{i}].Unit");
                        formData.Add(new StringContent(dto.ProductUnits[i].ConversionFactor.ToString()), $"ProductUnits[{i}].ConversionFactor");
                    }
                }

                if (ThumbnailImage != null && ThumbnailImage.Length > 0)
                {
                    Console.WriteLine($"ThumbnailImage: {ThumbnailImage.FileName}, Size: {ThumbnailImage.Length}");
                    var streamContent = new StreamContent(ThumbnailImage.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(ThumbnailImage.ContentType);
                    formData.Add(streamContent, "ThumbnailImage", ThumbnailImage.FileName);
                }
                else
                {
                    Console.WriteLine("No ThumbnailImage provided.");
                }

                var response = await _httpClient.PostAsync(ApiUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                    return Json(new { success = true, product = result?["product"] });
                }

                var rawResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Raw API Error Response: " + rawResponse);
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Create: " + ex.Message);
                return Json(new { success = false });
            }
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
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var product = await _httpClient.GetFromJsonAsync<ProductDto>($"{ApiUrl}/{id}");
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Index");
                }

                var units = await _httpClient.GetFromJsonAsync<List<ProductUnitDto>>($"{ApiUrl}/{id}/units");
                var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(CategoryApiUrl);
                ViewBag.Categories = categories?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName
                }).ToList() ?? new List<SelectListItem>();

                ViewBag.UnitSuggestions = await _httpClient.GetFromJsonAsync<List<string>>($"{ApiUrl}/unit-suggestions") ?? new List<string>();

                var model = new ProductUpdateDto
                {
                    Id = product.Id,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    SKU = product.SKU,
                    BaseUnit = product.BaseUnit,
                    RegularPrice = product.RegularPrice,
                    DiscountPrice = product.DiscountPrice,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    Barcode = product.Barcode,
                    UsageInstructions = product.UsageInstructions,
                    IsPrescriptionRequired = product.IsPrescriptionRequired,
                    IsPublished = product.IsPublished,
                    ThumbnailImagePath = product.ThumbnailImagePath,
                    ProductUnits = units?.Where(u => u.Unit != product.BaseUnit)
                                        .Select(u => new ProductUnitDto { Unit = u.Unit, ConversionFactor = u.ConversionFactor })
                                        .ToList() ?? new List<ProductUnitDto>()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi lấy dữ liệu sản phẩm: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateDto dto, IFormFile? ThumbnailImage)
        {
            if (!ModelState.IsValid)
                return await ReloadViewWithError(dto);

            try
            {
                // If no new image is uploaded, keep the existing ThumbnailImagePath
                // No file system operations are performed
                if (ThumbnailImage != null && ThumbnailImage.Length > 0)
                {
                    // Optional: Log that a new image was provided, but we won't process it
                    // You can extend this later if needed
                    TempData["ErrorMessage"] = "Chức năng upload ảnh mới hiện không được hỗ trợ.";
                    return await ReloadViewWithError(dto);
                }

                // Send the DTO to the API, retaining the existing ThumbnailImagePath
                var response = await _httpClient.PutAsJsonAsync(ApiUrl, dto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }

                var error = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Cập nhật sản phẩm thất bại: {error}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Cập nhật sản phẩm thất bại: {ex.Message}";
            }

            return await ReloadViewWithError(dto);
        }

        private async Task<IActionResult> ReloadViewWithError(ProductUpdateDto dto)
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(CategoryApiUrl);
            ViewBag.Categories = categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList() ?? new List<SelectListItem>();

            ViewBag.UnitSuggestions = await _httpClient.GetFromJsonAsync<List<string>>($"{ApiUrl}/unit-suggestions") ?? new List<string>();

            return View(dto);
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
