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
	}
}
