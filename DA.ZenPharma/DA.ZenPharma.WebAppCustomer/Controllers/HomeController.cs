using System.Diagnostics;
using System.Net.Http.Json;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.WebAppCustomer.Models;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAppCustomer.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7034/"); // Set base address for API
            _logger = logger;
        }
        public IActionResult Create()
        {
            return View();
        }



        public IActionResult Checkout()
        {
            return View();
        }



        public IActionResult Prescriptions()
        {
            return View();
        }


        public IActionResult ProductDetails()
        {
            return View();
        }

        public IActionResult Store()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Fetch products (top selling and discounted)
                var productResponse = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>("api/Product/paged?page=1&pageSize=20");
                var discountedProducts = await _httpClient.GetFromJsonAsync<PageResultDto<ProductDto>>("api/Product/paged?page=1&pageSize=20&isDiscounted=true");

                // Fetch categories
                var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/Category");

                if (productResponse == null || categories == null || discountedProducts == null)
                {
                    _logger.LogError("Failed to fetch data from API.");
                    return View("Error");
                }

                // Create view model
                var viewModel = new HomeViewModel
                {
                    TopSellingProducts = productResponse.Items,
                    DiscountedProducts = discountedProducts.Items,
                    Categories = categories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data for Home page.");
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // View model to hold data for the view
    public class HomeViewModel
    {
        public List<ProductDto> TopSellingProducts { get; set; }
        public List<ProductDto> DiscountedProducts { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }


}