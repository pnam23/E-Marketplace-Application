using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/Product")]
	public class ProductController:Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DataContext context,IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderBy(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}
		[Route("Create")]
		[HttpGet]
        public IActionResult Create()
        {
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
			return View();
		}
		[Route("Create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			
			if (ModelState.IsValid)
			{
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if(slug!=null)
				{
					ModelState.AddModelError("", "Sản phẩm đã tồn tại!");
					return View(product);
				}
				else
				{
					if(product.ImageUpload != null)
					{
						string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
						string imgName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
						string filePath = Path.Combine(uploadDir, imgName);

						FileStream fs = new FileStream(filePath, FileMode.Create);
						await product.ImageUpload.CopyToAsync(fs);
						fs.Close();
						product.Image = imgName;
					}
				}
				_dataContext.Add(product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm sản phẩm thành công!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model có một vài thứ đang bị lỗi!";
				List<string> errors = new List<string>();
				foreach(var value in ModelState.Values)
				{
					foreach(var err in value.Errors)
					{
						errors.Add(err.ErrorMessage);
					}
				}
				string errorMessage = string.Join('\n', errors);
				return BadRequest(errorMessage);
			}
			
			return View(product);
		}
		[Route("Edit")]
		public async Task<IActionResult> Edit(int id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(id);

			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
		
			return View(product);
		}
		[Route("Edit")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			var existedProduct = _dataContext.Products.Find(product.Id);

			if (ModelState.IsValid)
			{
				if (product.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imgName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imgName);

					//Delete old image
					string oldFilePath = Path.Combine(uploadDir, existedProduct.Image);

					try
					{
						if (System.IO.File.Exists(oldFilePath))
						{
							System.IO.File.Delete(oldFilePath);
						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("", "An error occurs while deleting the product image.");
					}

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					existedProduct.Image = imgName;

					
				}
				
				//Update other product properties
				existedProduct.Name = product.Name;
				existedProduct.Description = product.Description;
				existedProduct.Price = product.Price;
				existedProduct.CategoryId = product.CategoryId;
				existedProduct.BrandId = product.BrandId;

				//... other properties
				_dataContext.Update(existedProduct); //Update the existing product

				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật sản phẩm thành công!";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model có một vài thứ đang bị lỗi!";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var err in value.Errors)
					{
						errors.Add(err.ErrorMessage);
					}
				}
				string errorMessage = string.Join('\n', errors);
				return BadRequest(errorMessage);
			}

			return View(product);
		}
		[Route("Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(id);

			if(product == null)
			{
				return NotFound(); //Handle product not found
			}

			string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
			string oldFilePath=Path.Combine(uploadDir, product.Image);

			try
			{
				if (System.IO.File.Exists(oldFilePath))
				{
					System.IO.File.Delete(oldFilePath);
				}
			}
			catch(Exception ex)
			{
				ModelState.AddModelError("", "An error occurs while deleting the product image.");
			}

			_dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();
			TempData["error"] = "Đã xóa sản phẩm!";
			return RedirectToAction("Index");
		}
	}
}
