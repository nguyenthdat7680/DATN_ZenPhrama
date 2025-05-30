using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task AddAsync(CategoryCreateDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.Id = Guid.NewGuid();
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryUpdateDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);
            if (category == null) return;

            _mapper.Map(dto, category);
            _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;

            var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                return false; 
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        public async Task<PageResultDto<CategoryDto>> GetPagedAsync(int page, int pageSize, string? keyword = null, bool? isActive = null)
        {
            var query = _unitOfWork.Categories.GetAllForPaging();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.CategoryName.Contains(keyword));
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            query = query.OrderByDescending(c => c.CreateDate);

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PageResultDto<CategoryDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<CategoryDto>>(items)
            };
        }

    }
}
