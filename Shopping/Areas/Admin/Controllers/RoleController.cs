using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Role")]
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        
        private readonly DataContext _dataContext;
		private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
			_roleManager = roleManager;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderBy(r => r.Id).ToListAsync());
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
		public async Task<IActionResult> Create(IdentityRole model)
		{
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult()) { 
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index", "Role");
		}
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            try
            {
                await _roleManager.DeleteAsync(role);
                TempData["susccess"] = "Đã xóa Role thành công!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi xóa Role");
            }
            return Redirect("Index");
        }
		[HttpGet]
		[Route("Edit")]
		public async Task<IActionResult> Edit(string id)
		{
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
			return View(role);
		}
        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid) {
                var role = await _roleManager.FindByIdAsync(id);
                if(role == null)
                {
                    return NotFound();
                }

                role.Name = model.Name;

                try
                {
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật Role thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
					ModelState.AddModelError("", "Lỗi khi cập nhật Role");
				}
            }
            return View(model ?? new IdentityRole { Id = id });
        }
	}
}
