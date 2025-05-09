using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.RoleDtos;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(Guid id);
        Task AddAsync(RoleCreateDto dto);
        Task UpdateAsync(RoleUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<RoleDto>> GetPagedAsync(int page, int pageSize);
    }

}
