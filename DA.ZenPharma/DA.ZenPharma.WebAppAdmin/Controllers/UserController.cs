using DA.ZenPharma.Application.Dtos.UserDto;
using Microsoft.AspNetCore.Mvc;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/User";

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCreateDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, userDto);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Success = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Error = "Đăng ký thất bại. Vui lòng thử lại!";
            return View();
        }
    }
}
