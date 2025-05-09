using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.ProductDto;

namespace DA.ZenPharma.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task AddAsync(ProductCreateDto dto);
        Task UpdateAsync(ProductUpdateDto dto);
        Task DeleteAsync(Guid id);

        Task<PageResultDto<ProductDto>> GetProductPagedAsync(int page, int pageSize);
        Task<PageResultDto<ProductDto>> GetProductsByCategoryPagedAsync(Guid categoryId, int page, int pageSize);
        Task<List<ProductSearchDto>> SearchProductsAsync(string keyword, int take = 5);
    }
}
