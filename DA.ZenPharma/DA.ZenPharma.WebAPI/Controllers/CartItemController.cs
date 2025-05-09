using DA.ZenPharma.Application.Dtos.CartItemDto;
using DA.ZenPharma.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _cartItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _cartItemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartItemCreateDto dto)
        {
            await _cartItemService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartItemUpdateDto dto)
        {
            await _cartItemService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cartItemService.DeleteAsync(id);
            return Ok();
        }
    }
}
