using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;

namespace RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly MyBlogContext _context;

        public IndexModel(ILogger<IndexModel> logger, MyBlogContext context)
        {
            _context = context;
            _logger = logger;
        }

      


        public void OnGet()
        {


            var posts =_context.articles.OrderByDescending(x=>x.Created).ToList();
            ViewData["posts"] = posts;
        }
    }
}