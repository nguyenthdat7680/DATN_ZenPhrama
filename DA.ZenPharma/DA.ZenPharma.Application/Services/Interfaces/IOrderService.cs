using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.OrderDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task AddAsync(OrderCreateDto dto);
        Task UpdateAsync(OrderUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<OrderDto>> GetOrderPagedAsync(int page, int pageSize, string? branchId, string? status);
    }

}
