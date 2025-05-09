using DA.ZenPharma.Application.Dtos.ImportInvoiceDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportInvoiceController : ControllerBase
    {
        private readonly IImportInvoiceService _importInvoiceService;

        public ImportInvoiceController(IImportInvoiceService importInvoiceService)
        {
            _importInvoiceService = importInvoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _importInvoiceService.GetAllAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var invoice = await _importInvoiceService.GetByIdAsync(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ImportInvoiceUpdateDto dto)
        {
            await _importInvoiceService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _importInvoiceService.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedImportInvoice(int page = 1, int pageSize = 10)
        {
            var result = await _importInvoiceService.GetImportInvoicePagedAsync(page, pageSize);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ImportInvoiceCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _importInvoiceService.CreateAsync(dto);
            return Ok(new { message = "Thêm hóa đơn thành công" });
        }



    }
}
