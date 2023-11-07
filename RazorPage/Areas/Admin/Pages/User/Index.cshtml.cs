
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Areas.Admin.Pages.User
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }

        public class UserAndRole : AppUser
        {
            public string RoleNames { get; set; }
        }
        public List<UserAndRole> users { get; set; }
        public async Task OnGet()
        {
            users = await _userManager.Users.OrderBy(x => x.UserName).Select(u=>new UserAndRole()
            {
                Id =u.Id,
                UserName = u.UserName,
            }).ToListAsync();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames=string.Join(",", roles);
            }

        }

        public void OnPost() => RedirectToPage();
    }

}
