using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Repository;

namespace Shopping.Controllers
{
	public class ProductController: Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int id)
		{
			if (id == null) return RedirectToAction("Index");

			var producsById = _dataContext.Products.Where(p => p.Id == id).FirstOrDefault();

			return View(producsById);
		}

		public async Task<IActionResult>Search (string searchTerm)
		{
			var products = await _dataContext.Products
				.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
			ViewBag.Keyword = searchTerm;

			return View(products);
		}
	}

}
