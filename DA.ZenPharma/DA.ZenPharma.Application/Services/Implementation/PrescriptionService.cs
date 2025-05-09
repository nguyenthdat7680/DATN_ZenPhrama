using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.PrescriptionDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await _unitOfWork.Prescriptions.GetAllAsync();
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto?> GetByIdAsync(Guid id)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdAsync(id);
            return prescription == null ? null : _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task AddAsync(PrescriptionCreateDto dto)
        {
            var prescription = _mapper.Map<Prescription>(dto);
            await _unitOfWork.Prescriptions.AddAsync(prescription);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(PrescriptionUpdateDto dto)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdAsync(dto.Id);
            if (prescription == null) return;

            _mapper.Map(dto, prescription);
            await _unitOfWork.Prescriptions.UpdateAsync(prescription);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var prescription = await _unitOfWork.Prescriptions.GetByIdAsync(id);
            if (prescription == null) return;

            _unitOfWork.Prescriptions.Delete(prescription);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
