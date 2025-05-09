using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _branchService.GetAllAsync();
            return Ok(branches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var branch = await _branchService.GetByIdAsync(id);
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BranchCreateDto dto)
        {
            await _branchService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BranchUpdateDto dto)
        {
            await _branchService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _branchService.DeleteAsync(id);
            return Ok();
        }
    }
}
