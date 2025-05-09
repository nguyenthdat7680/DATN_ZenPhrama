using DA.ZenPharma.Application.Dtos.CartDto;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartCreateDto dto)
        {
            await _cartService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartUpdateDto dto)
        {
            await _cartService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cartService.DeleteAsync(id);
            return Ok();
        }
    }
}
