using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPage.Models
{
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Hãy đặt tên của sản phẩm")]
        public string? ProductName { get; set; }

        [DisplayName("Giá nhập sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập giá của sản phẩm")]
        public int? PriceIn { get; set; }

        [DisplayName("Giá nhập bán sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập giá bán của sản phẩm")]
        public int? PriceOut { get; set; }

        [DisplayName("Màu sắc sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập màu sắc của sản phẩm")]
        public string? Color { get; set; }
        [DisplayName("Mô tả sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập mô tả của sản phẩm")]

        public string? Decription { get; set; }
        public int? Insurance { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Discount { get; set; }
        public string? Material { get; set; }

        [DisplayName("Chiều dài sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập chiều dài của sản phẩm")]
        public int? Length { get; set; }
        [DisplayName("Chiều cao sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập chiều cao của sản phẩm")]
        public int? Height { get; set; }

        [DisplayName("Chiều rộng sản phẩm")]
        [Required(ErrorMessage = "Hãy nhập chiều rộng của sản phẩm")]
        public int? Depth { get; set; }
        public string? MadeIn { get; set; }
        public string? InstructionsForUse { get; set; }
        public string? ImageDefault { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
