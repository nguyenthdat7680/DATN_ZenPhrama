using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IImportInvoiceDetailService
    {
        Task<IEnumerable<ImportInvoiceDetailDto>> GetAllAsync();
        Task<ImportInvoiceDetailDto?> GetByIdAsync(Guid id);
        Task AddAsync(ImportInvoiceDetailCreateDto dto);
        Task UpdateAsync(ImportInvoiceDetailUpdateDto dto);
        Task DeleteAsync(Guid id);
    }

}
