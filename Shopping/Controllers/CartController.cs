using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Repository;

namespace Shopping.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			CartItemViewModel cartItemVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};

			return View(cartItemVM);
		}

		public IActionResult Checkout()
		{
			return View("~/Views/Checkout/Index.cshtml");
		}
		public async Task<IActionResult> Add(int id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c => c.ProductId == id).FirstOrDefault();

			if (cartItems == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItems.Quantity += 1;
			}
			HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Add Item to cart Successfully!";

			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Increase(int id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItems = cart.Where(c => c.ProductId == id).FirstOrDefault();
			if (cartItems.Quantity >= 1)
			{
				++cartItems.Quantity;
			}
			else
			{
				cart.RemoveAll(x => x.ProductId == id);
			}

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Increase Item quantity Successfully!";

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Decrease(int id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItems = cart.Where(c => c.ProductId == id).FirstOrDefault();
			
			if(cartItems.Quantity > 1)
			{
				--cartItems.Quantity;
			}
			else
			{
				cart.RemoveAll(x => x.ProductId == id);
			}

			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Decrease Item quantity Successfully!";

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Remove(int id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItems = cart.Where(c => c.ProductId == id).FirstOrDefault();

			cart.RemoveAll(x => x.ProductId == id);


			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Remove Item Successfully!";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear()
		{
			
			HttpContext.Session.Remove("Cart");

			TempData["success"] = "Clear all items Successfully!";

			return RedirectToAction("Index");
		}

	}
}	
