using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage="Vui lòng không bỏ trống")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Vui lòng không bỏ trống"), EmailAddress]
		public string Email {  get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Vui lòng không bỏ trống")]
		public string Password { get; set; }
	}
}
