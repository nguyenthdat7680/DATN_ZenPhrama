using System.Net.Http.Headers;
using System.Text.Json;
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
        private const string CategoryApiUrl = "https://localhost:7034/api/Category";

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>($"{ApiUrl}/paged?page={page}&pageSize={pageSize}");
            if (response == null) return View("Error");

            var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(CategoryApiUrl);
            ViewBag.Categories = categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList() ?? new List<SelectListItem>();

            ViewBag.UnitSuggestions = await _httpClient.GetFromJsonAsync<List<string>>($"{ApiUrl}/unit-suggestions") ?? new List<string>();

            ViewBag.TotalItems = response.TotalItems;
            ViewBag.PageSize = response.PageSize;
            ViewBag.TotalPages = response.TotalPages;
            ViewBag.Page = page;

            return View(response);
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
