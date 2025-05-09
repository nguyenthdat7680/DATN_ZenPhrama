using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.AddressDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAllAsync();
        Task<AddressDto?> GetByIdAsync(Guid id);
        Task AddAsync(AddressCreateDto dto);
        Task UpdateAsync(AddressUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
