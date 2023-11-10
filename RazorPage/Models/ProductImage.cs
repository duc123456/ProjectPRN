using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Models
{
    public partial class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Ảnh sản phẩm")]
        public string? Image { get; set; }
        [DisplayName("Mặc định")]
        public bool IsDefault { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }


    }
}
