using Microsoft.AspNetCore.Mvc;
using NhaThuocOnline.Intergration;
using NhaThuocOnline.ViewModel.Cart;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NhaThuocOnline.WebApp.Controllers
{
    public class CartController : ClientBaseController
    {
        private readonly ICartApiClient _cartApiClient;
        public CartController(ICartApiClient cartApiClient)
        {
            _cartApiClient = cartApiClient;
        }

        public async Task<IActionResult> Index()
        {
            if (!string.IsNullOrEmpty(sessions))
            {
                var customerIdClaim = GetCustomerIdFromToken(sessions);
                if (customerIdClaim != null)
                {
                    var cartId = await _cartApiClient.GetCartIdRecently(customerIdClaim).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(cartId)) 
                    {
                        var cartCustomer = await _cartApiClient.GetByCartId(cartId).ConfigureAwait(false);

                        //tổng tiền thanh toán
                        double totalPayment = 0;
                        foreach (var item in cartCustomer)
                        {
                            totalPayment += item.TotalPrice;
                        }
                        ViewBag.TotalPayment = totalPayment;
                        return View(cartCustomer);
                    }
                }
            }
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(int productId, int quantity)
        {
            if (!ModelState.IsValid)
                return View();

            var customerIdClaim = GetCustomerIdFromToken(sessions);

            if (customerIdClaim != null)
            {
                var cartId = await _cartApiClient.GetCartIdRecently(customerIdClaim).ConfigureAwait(false);
                var request= new CartCreateRequest()
                {
                     CartId = cartId,
                     CustomerId= Convert.ToInt32(customerIdClaim),
                     ProductId= productId,
                     Quantity = quantity
                };

                await _cartApiClient.CreateCartItem(request);
            }   
            return RedirectToAction("Search","Product");
        }


    }
}
