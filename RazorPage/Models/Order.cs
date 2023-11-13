using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPage.Models
{
	public class Order
	{
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]  
        public AppUser? AppUser { get; set; }   

        public string? Code { get; set; }
        [Required(ErrorMessage ="Hãy nhập tên người nhận hàng")]
        public string? Name { get; set; }
        public double OrderTotal { get; set; }

        public string? Comment { get; set; }
       
        public string? Status { get; set; }
		[Required(ErrorMessage = "Hãy nhập địa chỉ người nhận hàng")]
		public string? ShipAddress { get; set; }
        [Required(ErrorMessage = "Hãy nhập số điện thoại người nhận hàng")]
        public string? PhoneNumber { get; set; }
		[Required(ErrorMessage = "Hãy nhập Email người nhận hàng")]
		public string? Email { get; set; }

		public DateTime? UpdateDate { get; set; }
		public string? UpdateBy { get; set; }

		public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
