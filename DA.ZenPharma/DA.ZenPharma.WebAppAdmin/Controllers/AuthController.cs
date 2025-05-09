using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DA.ZenPharma.Domain.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "https://localhost:7034/api/Auth";

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password) 
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email và mật khẩu không được để trống!";
                    return View();
            }

            var requestBody = new { Email = email, PasswordHash = password };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(ApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }

            var responseData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            string token = responseData?.token;

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Không nhận được Token!";
                return View();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);


            string role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Staff";
            bool status = bool.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == "IsActive")?.Value, out bool isActive) && isActive;
            string userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            string userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            string branchId = jwtToken.Claims.FirstOrDefault(c => c.Type == "branchId")?.Value;
            string branchName = jwtToken.Claims.FirstOrDefault(c => c.Type == "branchName")?.Value;

            if (!status) 
            {
                ViewBag.Error = "Tài khoản của bạn chưa được kích hoạt!";
                return View();
            }

            HttpContext.Session.SetString("jwt", token);
            HttpContext.Session.SetString("userRole", role);
            HttpContext.Session.SetString("userId", userId);
            HttpContext.Session.SetString("userName", userName);

            if (!string.IsNullOrEmpty(branchId))
                HttpContext.Session.SetString("branchId", branchId);

            if (!string.IsNullOrEmpty(branchName))
                HttpContext.Session.SetString("branchName", branchName);

            // Đăng nhập vào hệ thống xác thực của MVC
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId ?? ""),
                new Claim(ClaimTypes.Name, userName ?? ""),
                new Claim(ClaimTypes.Role, role),
                new Claim("IsActive", status.ToString())
            };

            if (!string.IsNullOrEmpty(branchId))
                claims.Add(new Claim("branchId", branchId));

            if (!string.IsNullOrEmpty(branchName))
                claims.Add(new Claim("branchName", branchName));
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name,   
                ClaimTypes.Role    
            );
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
