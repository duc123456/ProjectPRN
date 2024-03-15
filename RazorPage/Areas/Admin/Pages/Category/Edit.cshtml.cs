using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RazorPage.Areas.Admin.Pages.Category
{
	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
    {
        private readonly MyBlogContext _context;
        private readonly IWebHostEnvironment _environment;
        public EditModel(MyBlogContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public class InputModel
        {
            [Display(Name = "Tên danh mục")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} tới {1} kí tự")]
            public string CategoryName { get; set; }

            [Display(Name = "Mô tả danh mục")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} tới {1} kí tự")]
            public string CategoryDescript { get; set; }

          

            [BindProperty]
            [DataType(DataType.Upload)]
            [DisplayName("Ảnh danh mục")]
            public IFormFile? fileImage { get; set; }
            public string? Image { get; set; }
        }


        public RazorPage.Models.Category category { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel input { get; set; }


        public async Task<IActionResult> OnGet(int cateId)
        {
            if (cateId == null)
            {
                return NotFound("Không tìm thấy danh mục");
            }
            category = await _context.Categories.FindAsync(cateId);
            if (category != null)
            {
                input = new InputModel
                {
                    CategoryName = category.CategoryName,
                    CategoryDescript  = category.Description,
                    Image = category.Image
                };
                return Page();
            }

            return NotFound("Không tìm thấy danh mục");
        }
        public async Task<IActionResult> OnPostAsync(int cateId)
        {
            if (cateId == null)
            {
                return NotFound("Không tìm thấy danh mục");
            }
            var cate = await _context.Categories.FindAsync(cateId);
            if (cate == null)
            {
                return NotFound("Không tìm thấy danh mục");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            cate.CategoryName = input.CategoryName;
            cate.Description = input.CategoryDescript;
            if(input.fileImage!= null)
            {
                var filePath = Path.Combine(_environment.WebRootPath, "img\\cate", input.fileImage.FileName);
                using var filestream = new FileStream(filePath, FileMode.Create);
                input.fileImage.CopyTo(filestream);
                cate.Image = "/img/cate/" + input.fileImage.FileName;
            }
            cate.UpdateDate = DateTime.Now;
            var result =  _context.Categories.Update(cate);
            _context.SaveChanges();
            StatusMessage = $"Bạn vừa đổi tên: {input.CategoryName}";
            return RedirectToPage("./Index");
        }
    }
}
