using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IInventoryBatchService
    {
        Task<List<InventoryBatchDto>> GetByProductIdAsync(Guid productId, Guid? branchId = null);
        Task<IEnumerable<InventoryBatchDto>> GetAllAsync();
        Task<InventoryBatchDto?> GetByIdAsync(Guid id);
        Task AddAsync(InventoryBatchCreateDto dto);
        Task UpdateAsync(InventoryBatchUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<InventoryBatchDto>> GetPagedAsync(int page, int pageSize, Guid? branchId, string? productName, string? batchName);

    }
}
