using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Vui lòng không bỏ trống")]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Vui lòng không bỏ trống")]
		public string Password { get; set; }
		public string ReturnUrl {  get; set; }
	}
}
