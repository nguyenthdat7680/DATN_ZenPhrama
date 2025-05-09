using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            var branches = await _unitOfWork.Branches.GetAllAsync();
            return _mapper.Map<IEnumerable<BranchDto>>(branches);
        }

        public async Task<BranchDto?> GetByIdAsync(Guid id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            return branch == null ? null : _mapper.Map<BranchDto>(branch);
        }

        public async Task AddAsync(BranchCreateDto dto)
        {
            var branch = _mapper.Map<Branch>(dto);
            await _unitOfWork.Branches.AddAsync(branch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(BranchUpdateDto dto)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(dto.Id);
            if (branch == null) return;

            _mapper.Map(dto, branch);
            _unitOfWork.Branches.UpdateAsync(branch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            if (branch == null) return;

            _unitOfWork.Branches.Delete(branch);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
