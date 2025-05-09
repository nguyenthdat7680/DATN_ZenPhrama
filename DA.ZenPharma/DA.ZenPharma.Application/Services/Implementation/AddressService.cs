using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.AddressDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            var addresses = await _unitOfWork.Addresses.GetAllAsync();
            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        public async Task<AddressDto?> GetByIdAsync(Guid id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            return address == null ? null : _mapper.Map<AddressDto>(address);
        }

        public async Task AddAsync(AddressCreateDto dto)
        {
            var address = _mapper.Map<Address>(dto);
            await _unitOfWork.Addresses.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(AddressUpdateDto dto)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(dto.Id);
            if (address == null) return;

            _mapper.Map(dto, address);  
            _unitOfWork.Addresses.UpdateAsync(address);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null) return;

            _unitOfWork.Addresses.Delete(address);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
