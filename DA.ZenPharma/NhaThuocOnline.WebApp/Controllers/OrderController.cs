using Microsoft.AspNetCore.Mvc;
using NhaThuocOnline.ApiIntergration;
using NhaThuocOnline.Intergration;
using NhaThuocOnline.ViewModel.Order;

namespace NhaThuocOnline.WebApp.Controllers
{
    public class OrderController : ClientBaseController
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly ICartApiClient _cartApiClient;
        private readonly ICustomerApiClient _customerApiClient;
        private readonly ICouponApiClient _couponApiClient;

        public OrderController(IOrderApiClient orderApiClient, ICartApiClient cartApiClient, ICustomerApiClient customerApiClient, ICouponApiClient couponApiClient)
        {
            _orderApiClient = orderApiClient;
            _cartApiClient = cartApiClient;
            _customerApiClient = customerApiClient;
            _couponApiClient = couponApiClient;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {

            if (sessions != null)
            {
                var customerId = GetCustomerIdFromToken(sessions);
                // Lấy id giỏ hàng 
                var cartId = await _cartApiClient.GetCartIdRecently(customerId);

                // lấy danh sách sản phẩm

                request.CustomerId = customerId;
                // xử lý đơn hàng
                await _orderApiClient.CreateOrder(request);

                return RedirectToAction("Checkout");
            }

            return NotFound();
        }


            [HttpGet]
        [Route("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            if(sessions!= null)
            {
                var customerId = GetCustomerIdFromToken(sessions);
                //lấy địa chỉ giao hàng
                var customerAddress = await _customerApiClient.GetCustomerAddresses(customerId);

                // Lấy sản phẩm từ giỏ hàng
                var cartId = await _cartApiClient.GetCartIdRecently(customerId);
                var productItemsInCart = await _orderApiClient.GetProductByCartId(cartId);
                // tính tổng tiền cần trả
                double totalPayment = 0;
                foreach (var item in productItemsInCart)
                {
                    totalPayment += item.TotalPrice;
                }
                ViewBag.TotalPayment = totalPayment;


                // dữ liệu trả về
                var result = new OrderItemCustomerAddress()
                {
                    CustomerAddressVm = customerAddress,
                    OrderItemVm = productItemsInCart,
                };

                if (result != null)
                {
                   
                    return View(result);

                }
            }
            
            return NotFound();

        }
    }
}
