using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RazorPage.Models;

namespace RazorPage.Areas.Identity.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly MyBlogContext _context;
        public IndexModel(MyBlogContext context) {

            _context = context;
        }
        public List<RazorPage.Models.Category> categories { get; set; }
        public void OnGet()
        {
            categories = _context.Categories.ToList();
        }
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult OnPostDeleteCategory(int cateId)
        {
            if(cateId == null)
            {
                return NotFound("Không tìm thấy danh mục");
            }
            var category = _context.Categories.Find(cateId);
            if(category == null) {
                return NotFound("Không tìm thấy danh mục");
            }

            StatusMessage = "Bạn đã xóa thành công "+ category.CategoryName;
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return new JsonResult(new {success = true, message="Delete Success"});
        }
    }
}
