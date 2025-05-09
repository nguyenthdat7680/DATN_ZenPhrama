using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.PrescriptionDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto?> GetByIdAsync(Guid id);
        Task AddAsync(PrescriptionCreateDto dto);
        Task UpdateAsync(PrescriptionUpdateDto dto);
        Task DeleteAsync(Guid id);
    }

}
