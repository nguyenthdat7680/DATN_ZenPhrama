using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.SupplierDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(Guid id);
        Task AddAsync(SupplierCreateDto dto);
        Task UpdateAsync(SupplierUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<SupplierDto>> GetSupplierPagedAsync(int page, int pageSize);
        Task<List<SupplierSearchDto>> SearchSuppliersAsync(string keyword, int take = 10);

    }

}
