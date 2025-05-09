using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.UserDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task AddAsync(UserCreateDto dto);
        Task UpdateAsync(UserUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<UserDto>> GetUserPagedAsync(int page, int pageSize);
        Task<PageResultDto<UserDto>> GetPagedStaffAsync(Guid roleId, int page, int pageSize);
    }
}
