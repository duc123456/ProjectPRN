using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Pages.Blog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RazorPage.Models.MyBlogContext _context;

        public IndexModel(RazorPage.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        public const int ITEMS_PER_PAGE = 10;
        [BindProperty(SupportsGet = true, Name ="p")]

        public int currentPage { get; set; }
        public int countPages { get; set; }

        public async Task OnGetAsync( string SearchString)
        {
            int total = await  _context.articles.CountAsync();
            countPages = (int)Math.Ceiling((double)total / ITEMS_PER_PAGE);

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > countPages)
            {
                currentPage = countPages;
            }
            var qr =  _context.articles.OrderByDescending(x => x.Created).Skip((currentPage-1)*ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE);

            if (!string.IsNullOrEmpty(SearchString))
            {
                Article = qr.Where(x => x.Title.ToLower().Contains(SearchString.ToLower())).ToList();
            }
            else
            {
                Article = await qr.ToListAsync();
            }

            

            
            
        }
    }
}
