using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Dtos.ProductUnitDtos;
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
            var products = await _unitOfWork.Products.GetAllForPaging()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetAllForPaging()
                .FirstOrDefaultAsync(p => p.Id == id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<Product> AddAsync(ProductCreateDto dto)
        {
            var baseUnit = dto.BaseUnit?.Trim();
            if (string.IsNullOrWhiteSpace(baseUnit))
                throw new Exception("Đơn vị cơ bản không được để trống.");

            var productUnits = dto.ProductUnits.Select(pu => pu.Unit?.Trim()).ToList();
            if (productUnits.Contains(baseUnit, StringComparer.OrdinalIgnoreCase) ||
                productUnits.Distinct(StringComparer.OrdinalIgnoreCase).Count() != productUnits.Count)
            {
                throw new Exception("Đơn vị trùng lặp!");
            }

            var product = _mapper.Map<Product>(dto);
            product.Id = Guid.NewGuid();
            product.BaseUnit = baseUnit;

            foreach (var unitDto in dto.ProductUnits)
            {
                if (string.IsNullOrWhiteSpace(unitDto.Unit))
                    throw new Exception("Đơn vị không được để trống.");

                product.ProductUnits.Add(new ProductUnit
                {
                    Id = Guid.NewGuid(),
                    Unit = unitDto.Unit.Trim(),
                    ConversionFactor = unitDto.ConversionFactor,
                    ProductId = product.Id
                });
            }

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(ProductUpdateDto dto)
        {
            // Tải sản phẩm với tracking
            var product = await _unitOfWork.Products.GetAllForPaging()
                .Include(p => p.ProductUnits) // Đảm bảo tải ProductUnits
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (product == null)
                throw new Exception($"Sản phẩm với ID {dto.Id} không tồn tại.");

            var baseUnit = dto.BaseUnit?.Trim();
            if (string.IsNullOrWhiteSpace(baseUnit))
                throw new Exception("Đơn vị cơ bản không được để trống.");

            var productUnits = dto.ProductUnits.Select(pu => pu.Unit?.Trim()).ToList();
            if (productUnits.Contains(baseUnit, StringComparer.OrdinalIgnoreCase) ||
                productUnits.Distinct(StringComparer.OrdinalIgnoreCase).Count() != productUnits.Count)
            {
                throw new Exception("Đơn vị trùng lặp!");
            }

            // Ánh xạ DTO sang entity
            _mapper.Map(dto, product);
            product.BaseUnit = baseUnit;

            // Xóa và thêm ProductUnits
            //await _unitOfWork.ProductUnits.RemoveRangeAsync(product.ProductUnits);
            //product.ProductUnits.Clear();
            //product.ProductUnits = dto.ProductUnits.Select(pu => new ProductUnit
            //{
            //    Id = Guid.NewGuid(),
            //    ProductId = product.Id,
            //    Unit = pu.Unit.Trim(),
            //    ConversionFactor = pu.ConversionFactor
            //}).ToList();

            // Đánh dấu entity là Modified
            _unitOfWork.Products.UpdateAsync(product);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                // Kiểm tra xem sản phẩm còn tồn tại không
                var exists = await _unitOfWork.Products.GetAllForPaging().AnyAsync(p => p.Id == dto.Id);
                if (!exists)
                    throw new Exception($"Sản phẩm với ID {dto.Id} không tồn tại.");
                throw; // Ném lại để xử lý ở tầng trên
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Sản phẩm không tồn tại.");

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PageResultDto<ProductDto>> GetProductPagedAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Products.GetAllForPaging()
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
            if (string.IsNullOrWhiteSpace(keyword)) return new List<ProductSearchDto>();

            var products = await _unitOfWork.Products.SearchByNameAsync(keyword, take);
            return _mapper.Map<List<ProductSearchDto>>(products);
        }

        public async Task<PageResultDto<ProductDto>> SearchProductsAsync(
            string? keyword, Guid? categoryId, string? orderBy, int page, int pageSize)
        {
            var (items, totalItems) = await _unitOfWork.Products.SearchAsync(keyword, categoryId, orderBy, page, pageSize);
            return new PageResultDto<ProductDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<ProductDto>>(items)
            };
        }

        public async Task<List<string>> GetUnitSuggestionsAsync()
        {
            var units = await _unitOfWork.Products.GetAllForPaging()
                .Select(p => p.BaseUnit)
                .Union((await _unitOfWork.ProductUnits.GetAllAsync()).Select(pu => pu.Unit))
                .Distinct()
                .ToListAsync();
            return units.Where(u => !string.IsNullOrWhiteSpace(u)).ToList();
        }

        public async Task<List<ProductUnitDto>> GetProductUnitsAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetAllForPaging()
                .Include(p => p.ProductUnits)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new Exception("Sản phẩm không tồn tại.");

            var units = new List<ProductUnitDto>
            {
                new ProductUnitDto
                {
                    Unit = product.BaseUnit,
                    ConversionFactor = 1 // Base unit has a conversion factor of 1
                }
            };

            units.AddRange(product.ProductUnits.Select(pu => new ProductUnitDto
            {
                Unit = pu.Unit,
                ConversionFactor = pu.ConversionFactor
            }));

            return units;
        }
    }
}
