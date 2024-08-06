using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập Username")]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
		public string Password { get; set; }
		public string ReturnUrl {  get; set; }
	}
}
