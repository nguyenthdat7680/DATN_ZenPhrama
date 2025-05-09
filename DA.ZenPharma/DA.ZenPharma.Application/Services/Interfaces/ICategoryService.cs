using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task AddAsync(CategoryCreateDto dto);
        Task UpdateAsync(CategoryUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<CategoryDto>> GetPagedAsync(int page, int pageSize, string? keyword = null, bool? isActive = null);

    }

}
