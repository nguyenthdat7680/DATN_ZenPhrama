using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDtos;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IImportInvoiceService
    {
        Task<IEnumerable<ImportInvoiceDto>> GetAllAsync();
        Task<ImportInvoiceDto?> GetByIdAsync(Guid id);
        Task AddAsync(ImportInvoiceCreateDto dto);
        //Task AddImportInvoiceAsync(ImportInvoiceCreateDto dto);
        Task UpdateAsync(ImportInvoiceUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PageResultDto<ImportInvoiceDto>> GetImportInvoicePagedAsync(int page, int pageSize);
        Task<Guid> CreateAsync(ImportInvoiceCreateDto dto);
    }

}
