using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/Category")]
    [Authorize(Roles = "Author, Publisher,Admin")]
    public class CategoryController:Controller
	{
		private readonly DataContext _dataContext;
		public  CategoryController(DataContext context)
		{
			_dataContext = context;
		}
        [Route("Index")]
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<CategoryModel> category = _dataContext.Categories.ToList();

            const int pageSize = 10; //10 items/trang

            if (pg < 1) //page < 1;
            {
                pg = 1; //page ==1
            }
            int recsCount = category.Count(); 

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            //category.Skip(20).Take(10).ToList()

            var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }
        [Route("Create")]
		public IActionResult Create()
		{
			return View();
		}
		[Route("Create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{
			if (ModelState.IsValid)
			{
				category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Danh mục đã tồn tại!");
					return View(category);
				}
			
				_dataContext.Add(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm danh mục thành công!";
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

			return View(category);
		}
		[Route("Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(id);

			if (category == null)
			{
				return NotFound(); //Handle product not found
			}

			_dataContext.Categories.Remove(category);
			await _dataContext.SaveChangesAsync();
			TempData["error"] = "Đã xóa danh mục!";
			return RedirectToAction("Index");
		}
		[Route("Edit")]
		public async Task<IActionResult> Edit(int id)
		{
			CategoryModel category = await _dataContext.Categories.FindAsync(id);

			return View(category);
		}
		[HttpPost]
		[Route("Edit")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryModel category)
		{
			var existedCategory = _dataContext.Categories.Find(category.Id);
			var oldSlug = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);

			if (ModelState.IsValid)
			{
				if (existedCategory.Name != category.Name)
				{
					//Update other category properties
					existedCategory.Name = category.Name;
					existedCategory.Slug = existedCategory.Name.Replace(" ", "-");
				}
				
				existedCategory.Description = category.Description;
				existedCategory.Status = category.Status;

				//... other properties
				_dataContext.Update(existedCategory); //Update the existing product

				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật danh mục thành công!";
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

			return View(category);
		}
	}
}
