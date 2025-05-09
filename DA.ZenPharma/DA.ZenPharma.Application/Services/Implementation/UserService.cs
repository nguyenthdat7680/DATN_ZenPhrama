using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.UserDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task AddAsync(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserUpdateDto dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.Id);
            if (user == null) return;

            _mapper.Map(dto, user);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return;

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PageResultDto<UserDto>> GetUserPagedAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Users.GetAllForPaging()
                .OrderByDescending(u => u.CreateDate);

            var totalItems = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<UserDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<UserDto>>(items)
            };
        }

        public async Task<PageResultDto<UserDto>> GetPagedStaffAsync(Guid roleId, int page, int pageSize)
        {
            var query = _unitOfWork.Users.GetAllForPaging()
                .Where(u => u.RoleId == roleId)
                .OrderByDescending(u => u.CreateDate);

            var totalItems = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<UserDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<UserDto>>(items)
            };
        }
    }
}
