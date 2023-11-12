
using System;
namespace RazorPage.Models
{
	public class CartItem
	{
       
        public int quantity { get; set; }
        public Product ProductItem { get; set; }
		
		public int? total { get; set; }

        public CartItem()
		{

		}
	}
}

