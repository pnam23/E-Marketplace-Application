using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class OrderModel
    {
        [Required]
        public int Id {  get; set; }
        public string OrderCode {  get; set; }
        public string UserName {  get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}
