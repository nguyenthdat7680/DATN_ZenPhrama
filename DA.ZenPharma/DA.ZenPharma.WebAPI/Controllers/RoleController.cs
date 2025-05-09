using DA.ZenPharma.Application.Dtos.RoleDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            await _roleService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto dto)
        {
            await _roleService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roleService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _roleService.GetPagedAsync(page, pageSize);
            return Ok(result);
        }
    }
}
