using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models
{
	public class RatingModel
	{
		[Key]
		public int Id {  get; set; }
		
		public int ProductId { get; set; }

		[Required(ErrorMessage = "* Yêu cầu nhập đánh giá sản phẩm")]
		public string Comment { get; set; }

		[Required(ErrorMessage = "* Yêu cầu nhập tên hiển thị")]
		public string Name { get; set; }

		public string Star { get; set; }

		[Required(ErrorMessage = "* Yêu cầu nhập Email")]
		
		public string Email {  get; set; }
		[ForeignKey("ProductId")]
		public ProductModel Product { get; set; }
	}
}
