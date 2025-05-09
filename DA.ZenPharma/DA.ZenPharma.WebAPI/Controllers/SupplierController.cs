using DA.ZenPharma.Application.Dtos.SupplierDto;
using DA.ZenPharma.Application.Services.Implementation;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SupplierController(ISupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _service.GetAllAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var supplier = await _service.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SupplierCreateDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SupplierUpdateDto dto)
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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedSuppliers(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetSupplierPagedAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? keyword = "", int take = 10)
        {
            var result = await _service.SearchSuppliersAsync(keyword ?? "", take);
            return Ok(result);
        }

    }
}
