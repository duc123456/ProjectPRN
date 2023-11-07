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
                Categories = _context.Categories.Select(
                n => new SelectListItem
                {
                    Value = n.CategoryId.ToString(),
                    Text = n.CategoryName.ToString(),
                }).ToList();
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
                    product.UpdateDate = DateTime.Now;
                    product.CreateDate = DateTime.Now;
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    StatusMessage = "Bạn vừa tạo thành công sản phẩm" + product.ProductName;
                }
            }
            return RedirectToPage("./Index");

        }
    }
}
