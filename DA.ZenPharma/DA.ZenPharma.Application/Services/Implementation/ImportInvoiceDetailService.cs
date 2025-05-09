using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class ImportInvoiceDetailService : IImportInvoiceDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImportInvoiceDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ImportInvoiceDetailDto>> GetAllAsync()
        {
            var details = await _unitOfWork.ImportInvoiceDetails.GetAllAsync();
            return _mapper.Map<IEnumerable<ImportInvoiceDetailDto>>(details);
        }

        public async Task<ImportInvoiceDetailDto?> GetByIdAsync(Guid id)
        {
            var detail = await _unitOfWork.ImportInvoiceDetails.GetByIdAsync(id);
            return detail == null ? null : _mapper.Map<ImportInvoiceDetailDto>(detail);
        }

        public async Task AddAsync(ImportInvoiceDetailCreateDto dto)
        {
            var detail = _mapper.Map<ImportInvoiceDetail>(dto);
            await _unitOfWork.ImportInvoiceDetails.AddAsync(detail);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImportInvoiceDetailUpdateDto dto)
        {
            var detail = await _unitOfWork.ImportInvoiceDetails.GetByIdAsync(dto.Id);
            if (detail == null) return;

            _mapper.Map(dto, detail);
            _unitOfWork.ImportInvoiceDetails.UpdateAsync(detail);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var detail = await _unitOfWork.ImportInvoiceDetails.GetByIdAsync(id);
            if (detail == null) return;

            _unitOfWork.ImportInvoiceDetails.Delete(detail);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
