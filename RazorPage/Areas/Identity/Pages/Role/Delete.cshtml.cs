using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel.DataAnnotations;

namespace App.Admin.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, MyBlogContext context) : base(roleManager, context)
        {
        }

 
        public IdentityRole Role { get; set; }



        public async Task<IActionResult> OnGet(string roleId)
        {
            if (roleId == null)
            {
                return NotFound("Không tìm thấy role");
            }
            Role = await _roleManager.FindByIdAsync(roleId);
            if (Role == null)
            {
                return NotFound("Không tìm thấy role");
            }

                return Page();
        }
        public async Task<IActionResult> OnPostAsync( string roleId)
        {
            if (roleId == null)
            {
                return NotFound("Không tìm thấy role");
            }
            Role = await _roleManager.FindByIdAsync(roleId);
            if (Role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var result =  await _roleManager.DeleteAsync(Role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa role: {Role.Name}";
                return RedirectToPage("./Index");
            }else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();
        }
    }
}
