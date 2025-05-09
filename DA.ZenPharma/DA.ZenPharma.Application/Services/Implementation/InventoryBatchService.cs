using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class InventoryBatchService : IInventoryBatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryBatchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<InventoryBatchDto>> GetByProductIdAsync(Guid productId)
        {
            var batches = await _unitOfWork.InventoryBatches.GetByProductIdAsync(productId);
            return _mapper.Map<List<InventoryBatchDto>>(batches);
        }

        public async Task<IEnumerable<InventoryBatchDto>> GetAllAsync()
        {
            var batches = await _unitOfWork.InventoryBatches.GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryBatchDto>>(batches);
        }

        public async Task<InventoryBatchDto?> GetByIdAsync(Guid id)
        {
            var batch = await _unitOfWork.InventoryBatches.GetByIdAsync(id);
            return batch == null ? null : _mapper.Map<InventoryBatchDto>(batch);
        }

        public async Task AddAsync(InventoryBatchCreateDto dto)
        {
            var entity = _mapper.Map<InventoryBatch>(dto);
            await _unitOfWork.InventoryBatches.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(InventoryBatchUpdateDto dto)
        {
            var entity = await _unitOfWork.InventoryBatches.GetByIdAsync(dto.Id);
            if (entity == null) return;

            _mapper.Map(dto, entity);
            _unitOfWork.InventoryBatches.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.InventoryBatches.GetByIdAsync(id);
            if (entity == null) return;

            _unitOfWork.InventoryBatches.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PageResultDto<InventoryBatchDto>> GetPagedAsync(int page, int pageSize, Guid? branchId = null, string? productName = null, string? batchName = null)
        {
            var query = _unitOfWork.InventoryBatches.GetAllForPaging()
                .Include(i => i.Product)
                .AsQueryable();

            if (branchId.HasValue)
            {
                query = query.Where(i => i.BranchId == branchId.Value);
            }
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(i => i.Product.ProductName.Contains(productName));
            }
            if (!string.IsNullOrWhiteSpace(batchName))
            {
                query = query.Where(i => i.BatchName.Contains(batchName));
            }

            query = query.OrderByDescending(i => i.CreateDate);
                
            var totalItems = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PageResultDto<InventoryBatchDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<InventoryBatchDto>>(data)
            };
        }

    }
}
