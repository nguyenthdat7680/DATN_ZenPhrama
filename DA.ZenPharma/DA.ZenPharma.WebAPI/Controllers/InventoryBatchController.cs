using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;
using DA.ZenPharma.Application.Services.Implementation;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryBatchController : ControllerBase
    {
        private readonly IInventoryBatchService _service;

        public InventoryBatchController(IInventoryBatchService service)
        {
            _service = service;
        }

        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetByProductId(Guid productId, [FromQuery] Guid? branchId = null)
        {
            var result = await _service.GetByProductIdAsync(productId, branchId);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? branchId = null, [FromQuery] string? productName = null, [FromQuery] string? batchName = null)
        {
            var result = await _service.GetPagedAsync(page, pageSize, branchId, productName, batchName);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InventoryBatchCreateDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InventoryBatchUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
