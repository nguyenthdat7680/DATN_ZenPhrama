using DA.ZenPharma.Application.Dtos.PrescriptionDto;
using DA.ZenPharma.Application.Dtos.SupplierDto;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prescriptions = await _prescriptionService.GetAllAsync();
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var prescription = await _prescriptionService.GetByIdAsync(id);
            if (prescription == null) return NotFound();
            return Ok(prescription);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PrescriptionCreateDto dto)
        {
            await _prescriptionService.AddAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PrescriptionUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _prescriptionService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _prescriptionService.DeleteAsync(id);
            return Ok();
        }
    }
}
