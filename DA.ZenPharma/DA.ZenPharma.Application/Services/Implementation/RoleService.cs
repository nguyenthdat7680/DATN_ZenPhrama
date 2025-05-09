using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.RoleDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetByIdAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            return role == null ? null : _mapper.Map<RoleDto>(role);
        }

        public async Task AddAsync(RoleCreateDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoleUpdateDto dto)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(dto.Id);
            if (role == null) return;

            _mapper.Map(dto, role);
            await _unitOfWork.Roles.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null) return;

            _unitOfWork.Roles.Delete(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PageResultDto<RoleDto>> GetPagedAsync(int page, int pageSize)
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();

            var paged = roles
                .OrderBy(r => r.RoleName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageResultDto<RoleDto>
            {
                Items = _mapper.Map<List<RoleDto>>(paged),
                TotalItems = roles.Count(),
                PageSize = pageSize
            };
        }

    }
}
