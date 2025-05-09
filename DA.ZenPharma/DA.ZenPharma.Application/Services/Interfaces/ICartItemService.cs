using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.CartItemDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItemDto>> GetAllAsync();
        Task<CartItemDto?> GetByIdAsync(Guid id);
        Task AddAsync(CartItemCreateDto dto);
        Task UpdateAsync(CartItemUpdateDto dto);
        Task DeleteAsync(Guid id);
    }

}
