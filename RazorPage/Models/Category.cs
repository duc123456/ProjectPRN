using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Models
{
    public partial class Category
    {

        [Key]
        public int CategoryId { get; set; }

        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0} phải dài từ {2} tới {1}")]
        [Required(ErrorMessage = "{0} phải nhập")]
        [DisplayName("Tên danh mục")]
        public string? CategoryName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

    }
}
