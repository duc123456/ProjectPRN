using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPage.Migrations;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Areas.Admin.Pages.Product
{
    public class EditModel : PageModel
    {
		private readonly MyBlogContext _context;
		private readonly IWebHostEnvironment _environment;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		public EditModel(MyBlogContext context, IWebHostEnvironment environment, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_context = context;
			_environment = environment;
			_userManager = userManager;
			_signInManager = signInManager;
		}


		public RazorPage.Models.Product product { get; set; }


        public class InputModel
        {
            [Display(Name = "Tên sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} tới {1} kí tự")]
            public string ProductName { get; set; }

            [Display(Name = "Giá nhập sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? PriceIn { get; set; }

            [Display(Name = "Giá bán sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? PriceOut { get; set; }

            [Display(Name = "Màu sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public string Color { get; set; }

            [Display(Name = "Màu sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public string Description { get; set; }
            [DisplayName("Số lượng sản phẩm")]
            [Required(ErrorMessage = "Hãy nhập số lượng của sản phẩm")]
            [Range(1, 99999, ErrorMessage = "Số lượng không hợp lệ")]
            public int Quantity { get; set; }

            [Display(Name = "Bảo hành sản phẩm (tháng)")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? Insurance { get; set; }

            [Display(Name = "Giảm giá sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? Discount { get; set; }

            [Display(Name = "Chất liệu sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public string Marterial { get; set; }

            [Display(Name = "Chiều dài sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? Length { get; set; }

            [Display(Name = "Chiều rộng sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? Depth { get; set; }

            [Display(Name = "Chiều cao sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? Height { get; set; }

            [Display(Name = "Xuất xứ sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public string MadeIn{ get; set; }

            [Display(Name = "Hướng dẫn sử dụng sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public string InstructionsForUse { get; set; }

            [BindProperty]
            [Display(Name = "Danh mục sản phẩm")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            public int? categoryId { get; set; }

            public List<SelectListItem>? Categories { get; set; }


            [BindProperty]
            [DataType(DataType.Upload)]
            [DisplayName("Ảnh danh mục")]
            public IFormFile? fileImage { get; set; }
            [BindProperty]
            public string Image { get; set; }
        }
        [BindProperty]
        public InputModel input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }





        public async Task<IActionResult> OnGet(int proId)
        {
            if (proId == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }
            product = await _context.Products.FindAsync(proId);
            if (product != null)
            {
                var Categories = _context.Categories.Select(
               n => new SelectListItem
               {
                   Value = n.CategoryId.ToString(),
                   Text = n.CategoryName.ToString(),
               }).ToList();
                input = new InputModel
                {
                    ProductName = product.ProductName,
                    PriceIn = product.PriceIn,
                    PriceOut = product.PriceOut,
                    Color = product.Color,
                    Description = product.Decription,
                    Insurance = product.Insurance,
                    Discount = product.Discount,
                    Marterial = product.Material,
                    Length = product.Length,
                    Height = product.Height,
                    Quantity = product.Quantity,
                    Depth = product.Depth,
                    MadeIn = product.MadeIn,
                    categoryId = product.CategoryId,
                    InstructionsForUse = product.InstructionsForUse,
                    Image = product.ImageDefault,
                    Categories = Categories,
                };
                return Page();
            }

            return NotFound("Không tìm thấy danh mục");
        }
        public async Task<IActionResult> OnPostAsync(int proId)
        {
            if (proId == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }
            var pro = await _context.Products.FindAsync(proId);
            if (pro == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            pro.ProductName= input.ProductName;
            pro.PriceIn= input.PriceIn;
            pro.PriceOut= input.PriceOut;
            pro.Color = input.Color;
            pro.Decription = input.Description;
            pro.Insurance = input.Insurance;
            pro.Discount = input.Discount;
            pro.Material = input.Marterial;
            pro.Length = input.Length;
            pro.Height = input.Height;
            pro.Depth = input.Depth;
            pro.Quantity = input.Quantity;
            pro.MadeIn = input.MadeIn;
            pro.InstructionsForUse = input.InstructionsForUse;
            pro.CategoryId = input.categoryId;
            if (input.fileImage != null)
            {
                var filePath = Path.Combine(_environment.WebRootPath, "img\\products", input.fileImage.FileName);
                using var filestream = new FileStream(filePath, FileMode.Create);
                input.fileImage.CopyTo(filestream);
                pro.ImageDefault= "/img/products/" + input.fileImage.FileName;
            }
			var user = await _userManager.GetUserAsync(User);
			product.UpdateBy = user.UserName;
			pro.UpdateDate = DateTime.Now;
            var result = _context.Products.Update(pro);
            _context.SaveChanges();
            StatusMessage = $"Bạn vừa đổi cập nhật: {input.ProductName}";
            return RedirectToPage("./Index");
        }
    }
}
