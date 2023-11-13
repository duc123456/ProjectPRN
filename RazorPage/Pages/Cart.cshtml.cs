using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		public CartModel(CartService cartService, MyBlogContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_cartService = cartService;
			_context = context;
			_userManager = userManager;
				_signInManager = signInManager;
		}
		[BindProperty]
		public decimal? Total { get; set; }
		[BindProperty]
		public List<CartItem> CartItems { get; set; }

		[TempData]
		public string StatusMessage { get; set; }


		[BindProperty]
		public Order order { get; set; }


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

		public async Task<IActionResult> OnPostCheckOutAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Redirect("/login/");
			}
			order.UserId = user.Id;
			if (!ModelState.IsValid)
			{
				return Page();
			}
			var cart = _cartService.GetCart();
			int? totalCart = cart.Select(x=>x.total).Sum(); 

			Guid obj = Guid.NewGuid();
            order.Code = obj.ToString();
			order.OrderTotal = (double)totalCart;
			order.Status = "Chờ xác nhận đơn hàng";
			order.UpdateDate = DateTime.Now;
            Order addOrder = order;
			_context.Orders.Add(addOrder);
			_context.SaveChanges();
			var caritems = _cartService.GetCart();
			foreach(var caritem in caritems)
			{
				OrderDetail orderDetail = new OrderDetail();
				orderDetail.OrderId = addOrder.Id;
				orderDetail.ProductId = caritem.ProductItem.ProductId;
				orderDetail.Name = caritem.ProductItem.ProductName;
				orderDetail.Quantity = caritem.quantity;
				orderDetail.Price =(double) caritem.ProductItem.PriceOut;
				_context.OrderDetails.Add(orderDetail);
				_context.SaveChanges(true);
			}
			_cartService.ClearCart();
			return RedirectToPage("Cart");
		}
	}
}
