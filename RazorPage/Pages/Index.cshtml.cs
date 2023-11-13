using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using RazorPage.Services;
using System.Security.Cryptography;

namespace RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly MyBlogContext _context;

		private readonly CartService _cartService;

        public IndexModel(ILogger<IndexModel> logger, CartService cartService, MyBlogContext context)
        {
			_cartService = cartService;
            _context = context;
            _logger = logger;
        }
		[BindProperty]
		public List<Category> categories { get; set; }
		[BindProperty]
		public List<Product> Products { get; set; }


        public const int ITEMS_PER_PAGE = 6;
        [TempData]
        public string StatusMessageSS { get; set; }

		[TempData]
		public string StatusMessageRR { get; set; }

		[BindProperty(SupportsGet = true, Name = "p")]

        public int currentPage { get; set; }

        public int countPages { get; set; }

        [BindProperty]
        public int CategoryIdSelected { get; set; }


        public async Task OnGet(int cateid = 0)
        {

			categories = _context.Categories.ToList();
			var product = _context.Products.Where(x => x.IsDeleted == false).ToList();

            if (cateid == 0)
            {
                int totalArticle = await _context.Products.CountAsync();
                countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_PER_PAGE);
                if (currentPage < 1) currentPage = 1;
                if (currentPage > countPages) currentPage = countPages;
                var listP = _context.Products.
                              Include(x => x.Category).
                              
                              OrderByDescending(x => x.ProductId).
                              Skip((currentPage - 1) * ITEMS_PER_PAGE).
                              Take(ITEMS_PER_PAGE);

                Products = listP.ToList();
            }
            else
            {
                CategoryIdSelected = cateid;
                int total = await _context.Products.Where(x => x.CategoryId == cateid).CountAsync();
                countPages = (int)Math.Ceiling((double)total / ITEMS_PER_PAGE);
                if (currentPage < 1) currentPage = 1;
                if (currentPage > countPages) currentPage = countPages;
                var listP = _context.Products.
                            Where(x => x.CategoryId == cateid).
                            OrderByDescending(x => x.ProductId).
                            Skip((currentPage - 1) * ITEMS_PER_PAGE).
                            Take(ITEMS_PER_PAGE);
                Products = listP.ToList();
            }
        }

		public IActionResult OnPostAddToCart(int productId, int price, int quantity)
		{
			if (quantity == 0)
			{
				StatusMessageSS = "Bạn đã không thêm sản phẩm nào";
				return RedirectToPage("Index");
			}
			// Thêm sản phẩm vào giỏ hàng
			Product product = _context.Products.Find(productId);
			if (product == null)
			{
				StatusMessageSS = "Sản phẩm này đã không còn";
				return RedirectToPage("Index");
			}
			product.PriceOut = price;

			var addCart = _cartService.AddToCart(product, quantity);
            if(addCart == true)
            {
				StatusMessageSS = $"Bạn vừa thêm {quantity} {product.ProductName} vào giỏ hàng";
			}else
            {
				StatusMessageRR = $"Có vẻ sản phẩm {product.ProductName} chúng tôi không có đủ trong kho. Hãy liên lạc với chung tôi qua mail để xử lí";

			}

			// Chuyển hướng về trang giỏ hàng
			
			return RedirectToPage("Index");
		}

	}
}