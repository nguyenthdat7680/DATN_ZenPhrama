using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Services.Implementation;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("{productId}/units")]
        public async Task<IActionResult> GetProductUnits(Guid productId)
        {
            try
            {
                var units = await _service.GetProductUnitsAsync(productId);
                return Ok(units);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetProductPagedAsync(page, pageSize);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductCreateDto dto)
        {
            _logger.LogInformation("Bắt đầu xử lý yêu cầu thêm sản phẩm.");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                _logger.LogWarning("ModelState không hợp lệ: {Errors}", errors);
                return BadRequest(new { success = false, errors });
            }

            try
            {
                _logger.LogInformation("Gọi _service.AddAsync với ProductCreateDto.");
                var product = await _service.AddAsync(dto);
                _logger.LogInformation("Thêm sản phẩm thành công, ID: {ProductId}", product.Id);

                return Ok(new
                {
                    success = true,
                    product = new
                    {
                        id = product.Id,
                        productCode = product.ProductCode,
                        productName = product.ProductName,
                        baseUnit = product.BaseUnit,
                        regularPrice = product.RegularPrice,
                        stockQuantity = product.StockQuantity,
                        thumbnailImagePath = product.ThumbnailImagePath
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm.");
                return BadRequest(new
                {
                    success = false,
                    errors = new Dictionary<string, string[]> { { "General", new[] { ex.Message } } }
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
        {
            _logger.LogInformation("Bắt đầu xử lý yêu cầu cập nhật sản phẩm ID: {ProductId}", dto.Id);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                _logger.LogWarning("ModelState không hợp lệ: {Errors}", errors);
                return BadRequest(new { success = false, errors });
            }

            try
            {
                await _service.UpdateAsync(dto);
                _logger.LogInformation("Cập nhật sản phẩm thành công, ID: {ProductId}", dto.Id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật sản phẩm.");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
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

            var result = await _service.SearchProductsAsync(keyword, categoryId, orderBy, page, pageSize);
            return Ok(result);
        }

        [HttpGet("search1")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var results = await _service.SearchProductsAsync(keyword);
            return Ok(results);
        }
        [HttpGet("unit-suggestions")]
        public async Task<IActionResult> GetUnitSuggestions()
        {
            var units = await _service.GetUnitSuggestionsAsync();
            return Ok(units);
        }
    }
}
