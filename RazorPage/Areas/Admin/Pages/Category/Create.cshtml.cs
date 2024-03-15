using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Areas.Admin.Pages.Category
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : PageModel
    {
        private readonly MyBlogContext _context;
        private readonly IWebHostEnvironment _environment;
        public CreateModel(MyBlogContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [BindProperty]
        
        public  RazorPage.Models.Category category { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Chọn ảnh để upload")]
        [DisplayName("Ảnh danh mục")]
        public IFormFile fileImage { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                if (fileImage != null)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "img\\cate", fileImage.FileName);
                    using var filestream = new FileStream(filePath, FileMode.Create);
                    fileImage.CopyTo(filestream);
                    category.Image = "/img/cate/" + fileImage.FileName;
                    category.UpdateDate = DateTime.Now;
                    category.CreateDate = DateTime.Now;
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    StatusMessage = "Bạn vừa tạo thành công danh mục " + category.CategoryName;
                }
            }
            return  RedirectToPage("./Index");

        }
    }
}
