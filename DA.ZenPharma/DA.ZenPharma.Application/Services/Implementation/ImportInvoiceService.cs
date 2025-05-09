using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class ImportInvoiceService : IImportInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImportInvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ImportInvoiceDto>> GetAllAsync()
        {
            var importInvoices = await _unitOfWork.ImportInvoices.GetAllAsync();
            return _mapper.Map<IEnumerable<ImportInvoiceDto>>(importInvoices);
        }

        public async Task<ImportInvoiceDto?> GetByIdAsync(Guid id)
        {
            var importInvoice = await _unitOfWork.ImportInvoices.GetById(id);
            return importInvoice == null ? null : _mapper.Map<ImportInvoiceDto>(importInvoice);
        }

        public async Task AddAsync(ImportInvoiceCreateDto dto)
        {
            var importInvoice = _mapper.Map<ImportInvoice>(dto);
            await _unitOfWork.ImportInvoices.AddAsync(importInvoice);
            await _unitOfWork.SaveChangesAsync();
        }
       
        public async Task UpdateAsync(ImportInvoiceUpdateDto dto)
        {
            var importInvoice = await _unitOfWork.ImportInvoices.GetByIdAsync(dto.Id);
            if (importInvoice == null) return;

            _mapper.Map(dto, importInvoice);
            await _unitOfWork.ImportInvoices.UpdateAsync(importInvoice);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var importInvoice = await _unitOfWork.ImportInvoices.GetByIdAsync(id);
            if (importInvoice == null) return;

            _unitOfWork.ImportInvoices.Delete(importInvoice);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PageResultDto<ImportInvoiceDto>> GetImportInvoicePagedAsync(int page, int pageSize)
        {
            var query = _unitOfWork.ImportInvoices.GetAllForPaging()
                .Include(i => i.Branch)
                .Include(i => i.Supplier)
                .Include(i => i.Staff)
                .OrderByDescending(i => i.CreateDate);

            var totalItems = await query.CountAsync();

            var importInvoices = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<ImportInvoiceDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<ImportInvoiceDto>>(importInvoices)
            };
        }

        public async Task<Guid> CreateAsync(ImportInvoiceCreateDto dto)
        {
            var invoice = _mapper.Map<ImportInvoice>(dto);
            invoice.InvoiceNumber = dto.InvoiceNumber ?? $"HDN-{DateTime.Now.Ticks}";
            
            foreach (var detailDto in dto.Details)
            {
                var detail = _mapper.Map<ImportInvoiceDetail>(detailDto);
                detail.ImportInvoiceId = invoice.Id;

                if (detailDto.Batches != null && detailDto.Batches.Any())
                {
                    foreach (var batchDto in detailDto.Batches)
                    {
                        var batch = _mapper.Map<InventoryBatch>(batchDto);
                        batch.ImportInvoiceDetailId = detail.Id;
                        batch.ProductId = detail.ProductId;
                        batch.BranchId = invoice.BranchLocationId;
                        detail.InventoryBatches.Add(batch);
                    }
                }

                invoice.Details.Add(detail);
            }

            invoice.TotalAmount = invoice.Details.Sum(x => x.TotalAmount);

            await _unitOfWork.ImportInvoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return invoice.Id;
        }

    }
}
