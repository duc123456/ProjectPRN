using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using RazorPage.Services;

namespace RazorPage.Pages
{
    public class CartModel : PageModel
    {
		private readonly CartService _cartService;
		private readonly MyBlogContext _context;
		public CartModel(CartService cartService, MyBlogContext context)
		{
			_cartService = cartService;
			_context = context;
		}
		public decimal? Total { get; set; }
		[BindProperty]
		public List<CartItem> CartItems { get; set; }

		[TempData]
		public string StatusMessage { get; set; }
		public void OnGet()
        {
			CartItems = _cartService.GetCart();
			Total = CartItems.Select(x => x.ProductItem.PriceOut * x.quantity).Sum();
		}
		public IActionResult OnPostAddToCart(int productId, int quantity)
		{
			var product = _context.Products.Find(productId);
			if (product == null)
			{
				return RedirectToPage("Cart");
			}
			_cartService.AddToCart(product, quantity);
			return RedirectToPage("Cart");
		}

		
	}
}
