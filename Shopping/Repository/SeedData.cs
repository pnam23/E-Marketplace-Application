using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context) {
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel tablet = new CategoryModel
				{
					Name = "Tablet",
					Slug = "tablet",
					Description = "tabletletletletlet",
					Status = 1
				};

				CategoryModel mobile = new CategoryModel
				{
					Name = "Mobile",
					Slug = "mobile",
					Description = "mobilelelelelelelle",
					Status = 1
				};

				BrandModel apple = new BrandModel { 
					Name="Apple",
					Slug="apple",
					Description="appleeeeee",
					Status = 1
				};

				BrandModel samsung = new BrandModel
				{
					Name = "Samsung",
					Slug = "samsung",
					Description = "samsungggg",
					Status = 1
				};

		

				BrandModel lenovo = new BrandModel
				{
					Name = "Lenovo",
					Slug = "lenovo",
					Description = "lenonononono",
					Status = 1
				};

				_context.Products.AddRange(
					new ProductModel
					{
						Name = "iPhone 15 128GB",
						Slug = "iphone-15-128gb",
						Description = "iPhone 15 128GB được trang bị màn hình Dynamic Island kích thước 6.1 inch với công nghệ hiển thị Super Retina XDR màn lại trải nghiệm hình ảnh vượt trội. Điện thoại với mặt lưng kính nhám chống bám mồ hôi cùng 5 phiên bản màu sắc lựa chọn: Hồng, Vàng, Xanh lá, Xanh dương và đen. Camera trên iPhone 15 series cũng được nâng cấp lên cảm biến 48MP cùng tính năng chụp zoom quang học tới 2x. Cùng với thiết kế cổng sạc thay đổi từ lightning sang USB-C vô cùng ấn tượng.!",
						Image = "iphone-15.jpg",
						Price = 999,
						Category = mobile,
						Brand = apple,
					},
					new ProductModel
					{
						Name = "Samsung Galaxy Z Fold6",
						Slug = "samsung-galaxy-z-fold6",
						Description = "Samsung Z Fold 6 là siêu phẩm điện thoại gập được ra mắt ngày 10/7, hiệu năng dẫn đầu phân khúc với chip 8 nhân Snapdragon 8 Gen 3 for Galaxy, 12GB RAM cùng bộ nhớ trong từ 256GB đến 1TB. Thay đổi mạnh mẽ về hiệu năng và thiết kế, Galaxy Z Fold 6 hứa hẹn sẽ là chiếc smartphone AI đáng sở hữu nhất nửa cuối năm 2024.",
						Image = "samsung-galaxy-z-fold-6.jpg",
						Price = 1999,
						Category = mobile,
						Brand = samsung,
					},
					new ProductModel
					{
						Name = "Lenovo Tab K11",
						Slug = "lenovo-tab-k11",
						Description = "Trải nghiệm hình ảnh sống động, sắc nét và chi tiết với màn hình lớn 11 inch và chứng nhận TÜV Rheinland Low Blue Light.\r\nTận hưởng âm thanh sống động, chân thực với hệ thống 4 loa JBL cùng công nghệ Dolby Atmos.\r\nSử dụng thoải mái cả ngày dài với pin dung lượng lớn 7040mAh, cho bạn xem phim, chơi game và làm việc liên tục mà không lo hết pin.\r\nSoC MediaTek Helio G88 cho khả năng xử lý đa nhiệm mượt mà, xem phim, chơi game và làm việc hiệu quả.",
						Image = "lenovo-tab-k11.jpg",
						Price = 299,
						Category = tablet,
						Brand = lenovo,
					}
				);
			}
			_context.SaveChanges();
		}
	}
}
