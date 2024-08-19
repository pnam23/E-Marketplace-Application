using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping.Areas.Admin.Repository;
using Shopping.Models;
using Shopping.Models.ViewModels;

namespace Shopping.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		private readonly IEmailSender _emailSender;

		public AccountController(IEmailSender emailSender,SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_emailSender = emailSender;
		}
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM) 
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					TempData["success"] = "Đăng nhập thành công!";
					var reciever = "renducluyentai666@gmail.com";
					var subject = "Đăng nhập trên thiết bị thành công!";
					var message = "Đăng nhập thành công, chúc bạn trải nghiệm dịch vụ vui vẻ!";

					await _emailSender.SendEmailAsync(reciever, subject, message);
					return Redirect(loginVM.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
			}
			return View(loginVM);
		}

		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(UserModel user)
		{
			if(ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel
				{
					UserName = user.UserName,
					Email = user.Email,
				};
				IdentityResult result = await _userManager.CreateAsync(newUser,user.Password);
				if (result.Succeeded)
				{
					TempData["success"] = "Đăng ký user mới thành công!";
					return Redirect("/account/login");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View();
		}

		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}
	}
}
