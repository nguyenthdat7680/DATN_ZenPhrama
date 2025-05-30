using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NhaThuocOnline.WebApp.Controllers
{
    public class ClientBaseController : Controller
    {
        protected string sessions
        {
            get
            {
                return HttpContext.Session.GetString("Token");
            }
        }

        protected static int GetCustomerIdFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var customerId = token.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == ClaimTypes.NameIdentifier);
            return Convert.ToInt32(customerId.Value);
        }

    }
}
