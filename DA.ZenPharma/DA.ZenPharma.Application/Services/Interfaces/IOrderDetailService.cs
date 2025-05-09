using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.OrderDetailDtos;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDto>> GetAllAsync();
        Task<OrderDetailDto?> GetByIdAsync(Guid id);
        Task AddAsync(OrderDetailCreateDto dto);
        Task UpdateAsync(OrderDetailUpdateDto dto);
        Task DeleteAsync(Guid id);
    }

}
