using RazorPage.Models;
using System;
using System.Text.Json;


namespace RazorPage.Services
{
	public class CartService
	{
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartCookieName = "CartIsofa";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool AddToCart(Product item, int quantity)
        {
            var cart = GetCartFromCookie();

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
            var existingItem = cart.FirstOrDefault(i => i.ProductItem.ProductId == item.ProductId);
            if (existingItem != null)
            {
                // Nếu sản phẩm đã tồn tại, tăng số lượng lên
                existingItem.quantity += quantity;
                existingItem.total += item.PriceOut * quantity;
                if(existingItem.quantity > item.Quantity) { 
                    return false;
                }
				if (existingItem.quantity == 0)
                {
                    cart.Remove(existingItem);
                }
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại, thêm vào giỏ hàng
                item.Decription = "";
                cart.Add(new CartItem { ProductItem = item, quantity = quantity, total = item.PriceOut*quantity }) ;
            }
            // Lưu giỏ hàng vào cookie
            SaveCartToCookie(cart);
            return true;
        }

        public List<CartItem> GetCart()
        {
            return GetCartFromCookie();
        }

        public void RemoveItem(CartItem item)
        {
			var cart = GetCartFromCookie();
            cart.Remove(item);
			SaveCartToCookie(cart);
		}

        public void ClearCart()
        {
            // Xóa giỏ hàng trong cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_cartCookieName);
        }

        private List<CartItem> GetCartFromCookie()
        {
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[_cartCookieName];
            if (!string.IsNullOrEmpty(cartJson))
            {
                // Chuyển đổi dữ liệu JSON trong cookie thành danh sách sản phẩm
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }
            return new List<CartItem>();
        }

        private void SaveCartToCookie(List<CartItem> cart)
        {
            //var cartJson = JsonConvert.SerializeObject(cart);
            var cartJson = JsonSerializer.Serialize(cart);

			// Lưu giỏ hàng vào cookie
			_httpContextAccessor.HttpContext.Response.Cookies.Append(_cartCookieName, cartJson, new CookieOptions
            {
                // Đặt thời gian sống của cookie
                Expires = DateTimeOffset.Now.AddDays(7)
            });
        }
    }
}

