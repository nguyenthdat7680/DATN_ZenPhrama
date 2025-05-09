using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BranchDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<BranchDto?> GetByIdAsync(Guid id);
        Task AddAsync(BranchCreateDto dto);
        Task UpdateAsync(BranchUpdateDto dto);
        Task DeleteAsync(Guid id);
    }



}
