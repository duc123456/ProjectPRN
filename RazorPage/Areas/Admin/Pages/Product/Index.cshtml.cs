using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Areas.Admin.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly MyBlogContext _context;
        public IndexModel(MyBlogContext context) {

            _context = context;
        }
        public List<RazorPage.Models.Product> products { get; set; }
        public void OnGet()
        {
            products = _context.Products.Include(x=>x.Category).ToList();
        }
        [TempData]
        public string StatusMessage { get; set; }
        public IActionResult OnPostDeleteProduct(int productId)
        {
            if(productId == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }
            var product = _context.Products.Find(productId);
            if(product == null) {
                return NotFound("Không tìm thấy sản phẩm");
            }

            StatusMessage = "Bạn đã xóa thành công "+ product.ProductName;
            _context.Products.Remove(product);
            _context.SaveChanges();
            return new JsonResult(new {success = true, message="Delete Success"});
        }
    }
}
