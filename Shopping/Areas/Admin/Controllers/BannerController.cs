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
    [Route("Admin/Banner")]
    [Authorize(Roles = "Publisher,Admin")]
    public class BannerController : Controller
    {
        private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public BannerController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
			_webHostEnvironment = webHostEnvironment;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Banners.OrderByDescending(p => p.Id).ToListAsync());
        }
		[Route("Create")]
		[HttpGet]
		public IActionResult Create()
		{
			
			return View();
		}
		[Route("Create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BannerModel banner)
		{
			if (ModelState.IsValid)
			{
				if (banner.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/banners");
					string imgName = Guid.NewGuid().ToString() + "_" + banner.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imgName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await banner.ImageUpload.CopyToAsync(fs);
					fs.Close();
					banner.Image = imgName;
				}

				_dataContext.Add(banner);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm banner thành công!";
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
		}
		[Route("Edit")]
		public async Task<IActionResult> Edit(int id)
		{
			BannerModel banner = await _dataContext.Banners.FindAsync(id);

			return View(banner);
		}
		[Route("Edit")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(BannerModel banner)
		{
			
			var existedBanner = _dataContext.Banners.Find(banner.Id);

			if (ModelState.IsValid)
			{
				if (banner.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/banners");
					string imgName = Guid.NewGuid().ToString() + "_" + banner.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imgName);

					//Delete old image
					string oldFilePath = Path.Combine(uploadDir, existedBanner.Image);

					try
					{
						if (System.IO.File.Exists(oldFilePath))
						{
							System.IO.File.Delete(oldFilePath);
						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("", "An error occurs while deleting the banner image.");
					}

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await banner.ImageUpload.CopyToAsync(fs);
					fs.Close();
					existedBanner.Image = imgName;


				}

				//Update other banner properties
				existedBanner.Name = banner.Name;
				existedBanner.Description = banner.Description;
				existedBanner.Status = banner.Status;

				//... other properties
				_dataContext.Update(existedBanner); //Update the existing banner

				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật banner thành công!";
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
		}
		[Route("Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			BannerModel banner = await _dataContext.Banners.FindAsync(id);

			if (banner == null)
			{
				return NotFound(); //Handle product not found
			}

			string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/banners");
			string oldFilePath = Path.Combine(uploadDir, banner.Image);

			try
			{
				if (System.IO.File.Exists(oldFilePath))
				{
					System.IO.File.Delete(oldFilePath);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "An error occurs while deleting the banner image.");
			}

			_dataContext.Banners.Remove(banner);
			await _dataContext.SaveChangesAsync();
			TempData["error"] = "Đã xóa banner!";
			return RedirectToAction("Index");
		}
	}
}
