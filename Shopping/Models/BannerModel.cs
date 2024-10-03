using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shopping.Repository.Validation;

namespace Shopping.Models
{
	public class BannerModel
	{
		
		public int Id { get; set; }

		[Required(ErrorMessage = "* Yêu cầu không bỏ trống tên banner")]
		public string Name { get; set; }

		[Required(ErrorMessage = "* Yêu cầu không bỏ trống mô tả")]
		public string Description { get; set; }
		public int? Status { get; set; }
		public string Image {  get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }
	}
}
