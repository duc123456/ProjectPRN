using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Areas.Admin.Pages.Product
{
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

        public RazorPage.Models.Product product { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [DisplayName("Danh mục")]
        [Required(ErrorMessage = "Chọn danh mục của sản phẩm")]
        [BindProperty]
        public int CategoryIdSelected { get; set; }

        [BindProperty]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Chọn ảnh để upload")]
        [DisplayName("Ảnh sản phẩm")]
        public IFormFile fileImage { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public void OnGet()
        {
            Categories = _context.Categories.Select(
            n => new SelectListItem
            {
                Value = n.CategoryId.ToString(),
                Text = n.CategoryName.ToString(),
            }).ToList();
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
                    var filePath = Path.Combine(_environment.WebRootPath, "img\\products", fileImage.FileName);
                    using var filestream = new FileStream(filePath, FileMode.Create);
                    fileImage.CopyTo(filestream);
                    product.ImageDefault = "/img/products/" + fileImage.FileName;
                    //category.UpdateDate = DateTime.Now;
                    //category.CreateDate = DateTime.Now;
                    //_context.Categories.Add(category);
                    //_context.SaveChanges();
                    //StatusMessage = "Bạn vừa tạo thành công danh mục " + category.CategoryName;
                }
            }
            return RedirectToPage("./Index");

        }
    }
}
