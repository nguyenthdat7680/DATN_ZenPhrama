using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.CartDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAllAsync();
        Task<CartDto?> GetByIdAsync(Guid id);
        Task AddAsync(CartCreateDto dto);
        Task UpdateAsync(CartUpdateDto dto);
        Task DeleteAsync(Guid id);
    }


}
