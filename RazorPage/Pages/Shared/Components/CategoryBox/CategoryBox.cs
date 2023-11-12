using Microsoft.AspNetCore.Mvc;
using RazorPage.Models;

namespace RazorPage.Pages.Shared.Components.CategoryBox
{
	public class CategoryBox : ViewComponent
	{
		private readonly ILogger<IndexModel> _logger;

		private readonly MyBlogContext _context;

		public CategoryBox(ILogger<IndexModel> logger, MyBlogContext context)
		{
			_context = context;
			_logger = logger;
		}
		public IViewComponentResult Invoke()
		{
			var category =  _context.Categories.ToList();
			return View<List<Category>>(category);
		}
	}
}
