using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using RazorPage.Services;
using System.Linq;

namespace RazorPage.Pages
{
    public class DetailModel : PageModel
    {
		private readonly ILogger<IndexModel> _logger;

		private readonly MyBlogContext _context;
		private readonly CartService _cartService;

		public DetailModel(ILogger<IndexModel> logger, CartService cartService, MyBlogContext context)
		{
			_context = context;
			_logger = logger;
			_cartService = cartService;
		}
		[BindProperty]
		public Product Product { get; set; }
        [TempData]
        public string StatusMessageSS { get; set; }

        [TempData]
        public string StatusMessageRR { get; set; }

        [BindProperty]
		public List<Product> SameProduct { get; set; }
		public void OnGet(int? proId)
        {
			if(proId != null)
			{
				Product = _context.Products.Include(x=>x.ProductImages).FirstOrDefault(x => x.ProductId == proId);
				SameProduct = _context.Products.Where(x=>x.CategoryId == Product.CategoryId && x.ProductId != proId).Take(4).ToList();
			}

        }
		public IActionResult OnPostAddToCart(int productId, int price, int quantity)
		{
			if (quantity == 0)
			{
                StatusMessageRR = "Bạn đã không thêm sản phẩm nào";
				return RedirectToPage("Index");
			}
			// Thêm sản phẩm vào giỏ hàng
			Product product = _context.Products.Find(productId);
			if (product == null)
			{
                StatusMessageRR = "Sản phẩm này đã không còn";
				return RedirectToPage("Index");
			}
			product.PriceOut = price;

            var addCart = _cartService.AddToCart(product, quantity);
            if (addCart == true)
            {
                StatusMessageSS = $"Bạn vừa thêm {quantity} {product.ProductName} vào giỏ hàng";
            }
            else
            {
                StatusMessageRR = $"Có vẻ sản phẩm {product.ProductName} chúng tôi không có đủ trong kho. Hãy liên lạc với chung tôi qua mail để xử lí";

            }

          
			return RedirectToPage("/Detail" , productId);
		}
	}
}
