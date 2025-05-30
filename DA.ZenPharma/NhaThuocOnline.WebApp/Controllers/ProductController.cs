using Microsoft.AspNetCore.Mvc;
using NhaThuocOnline.ApiIntergration;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;
using NhaThuocOnline.ViewModel.Product;

namespace NhaThuocOnline.WebApp.Controllers
{
    public class ProductController : ClientBaseController
    {
        private readonly IProductApiClient _productApiClient;
        public ProductController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? keyword, string? orderBy, int? categoryId, int pageIndex = 1, int pageSize = 8)
        {
            var request = new GetPublicProductPagingRequest()
            {
                Keyword = keyword,
                OrderBy = orderBy,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var result = await _productApiClient.GetProductByCategoryIdPaging(request);

            ViewBag.TotalRecords = result.TotalRecords;
            ViewBag.PageSize = result.PageSize;
            ViewBag.PageCount = result.PagedCount;
            ViewBag.PageIndex = result.PageIndex;
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productApiClient.GetProductById(id);
            return View(result);
        }

      
    }
}
