using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Repository;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Order")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [Route("Index")]
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Orders.OrderBy(p => p.Id).ToListAsync());
		}
        [HttpGet]
        [Route("ViewDetails")]
        public async Task<IActionResult> ViewDetails(string ordercode)
        {
            var detailsOrder = await _dataContext.OrderDetail.Include(ord => ord.Product).Where(ord=>ord.OrderCode==ordercode).ToListAsync();
            //return View(await _dataContext.Orders.OrderBy(o => o.Id).ToListAsync());
            return View(detailsOrder);
        }
        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o=>o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the order status!");
            }
        }
	}
}
