using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPage.Models
{
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? PriceIn { get; set; }
        public int? PriceOut { get; set; }
        public string? Color { get; set; }
        public string? Decription { get; set; }
        public int? Insurance { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Discount { get; set; }
        public string? Material { get; set; }
        public int? Length { get; set; }
        public int? Height { get; set; }
        public int? Depth { get; set; }
        public string? MadeIn { get; set; }
        public string? InstructionsForUse { get; set; }
        public string? ImageDefault { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
