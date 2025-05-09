using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task AddAsync(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductUpdateDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.Id);
            if (product == null) return;

            _mapper.Map(dto, product); 
            _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return;

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<PageResultDto<ProductDto>> GetProductPagedAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Products.GetAllForPaging()
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDate);

            var totalItems = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<ProductDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<ProductDto>>(products)
            };
        }

        public async Task<PageResultDto<ProductDto>> GetProductsByCategoryPagedAsync(Guid categoryId, int page, int pageSize)
        {
            var query = _unitOfWork.Products.GetAllForPaging()
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreateDate);

            var totalItems = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<ProductDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<ProductDto>>(products)
            };
        }
        public async Task<List<ProductSearchDto>> SearchProductsAsync(string keyword, int take = 5)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new();

            var products = await _unitOfWork.Products.SearchByNameAsync(keyword, take);
            return _mapper.Map<List<ProductSearchDto>>(products);
        }

    }
}
