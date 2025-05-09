using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.SupplierDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto?> GetByIdAsync(Guid id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            return supplier == null ? null : _mapper.Map<SupplierDto>(supplier);
        }

        public async Task AddAsync(SupplierCreateDto dto)
        {
            var supplier = _mapper.Map<Supplier>(dto);
            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(SupplierUpdateDto dto)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(dto.Id);
            if (supplier == null) return;

            _mapper.Map(dto, supplier);
            await _unitOfWork.Suppliers.UpdateAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (supplier == null) return;

            _unitOfWork.Suppliers.Delete(supplier);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<PageResultDto<SupplierDto>> GetSupplierPagedAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Suppliers.GetAllForPaging()
                .OrderByDescending(s => s.CreateDate);

            var totalItems = await query.CountAsync();

            var suppliers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<SupplierDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<SupplierDto>>(suppliers)
            };
        }

        public async Task<List<SupplierSearchDto>> SearchSuppliersAsync(string? keyword, int take = 10)
        {
            var suppliers = await _unitOfWork.Suppliers.SearchByNameAsync(keyword ?? "", take);
            return _mapper.Map<List<SupplierSearchDto>>(suppliers);
        }


    }
}
