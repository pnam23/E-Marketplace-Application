using Microsoft.AspNetCore.Mvc;
using Shopping.Areas.Admin.Repository;
using Shopping.Models;
using Shopping.Repository;
using System.Security.Claims;

namespace Shopping.Controllers
{
	public class CheckoutController:Controller
	{
		private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        public CheckoutController(IEmailSender emailSender,DataContext dataContext)
		{
			_dataContext = dataContext;
			_emailSender = emailSender;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = ordercode;
				orderItem.UserName= userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;

				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

				foreach (var cart in cartItems)
				{
					var orderDetail = new OrderDetail();
					orderDetail.UserName = userEmail;
					orderDetail.OrderCode = ordercode;
					orderDetail.ProductId = cart.ProductId;
					orderDetail.Price = cart.Price;
					orderDetail.Quantity = cart.Quantity;
					_dataContext.Add(orderDetail);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");

                var reciever = userEmail;
                var subject = "Đặt hàng thành công!";
                var message = "Đặt hàng thành công, chúc bạn trải nghiệm dịch vụ vui vẻ!";

                await _emailSender.SendEmailAsync(reciever, subject, message);


                TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng!";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}
	}
}
