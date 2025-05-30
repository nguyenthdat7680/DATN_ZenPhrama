using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NhaThuocOnline.ApiIntergration;
using NhaThuocOnline.ViewModel.Customer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NhaThuocOnline.WebApp.Controllers
{
    public class CustomerController : ClientBaseController
    {
        private readonly ICustomerApiClient _customerApiClient;
        public CustomerController(ICustomerApiClient customerApiClient)
        {
            _customerApiClient = customerApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CustomerAddresses(int customerId) {
            var result= await _customerApiClient.GetCustomerAddresses(customerId);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _customerApiClient.Authenticate(request);
            if(result != null)
            {
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", result.ResultObject);
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            if (!string.IsNullOrEmpty(sessions))
            {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(sessions);

            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                await Console.Out.WriteLineAsync(emailClaim.ToString());
            }

            return View();

            }
            return NotFound();
        }

    }
}
