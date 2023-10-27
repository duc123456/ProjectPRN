using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using System.ComponentModel.DataAnnotations;

namespace App.Admin.Role
{

    [Authorize(Policy ="AllowEditRole")]
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, MyBlogContext context) : base(roleManager, context)
        {
        }


        public class InputModel
        {
            [Display(Name = "Tên của role")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} tới {1} kí tự")]
            public string Name { get; set; }
        }

        public IdentityRole Role { get; set; }

        public List<IdentityRoleClaim<string>> Claims { get; set; }


        [BindProperty]
        public InputModel input { get; set; }
        public async Task<IActionResult> OnGet(string roleId)
        {
            if (roleId == null)
            {
                return NotFound("Không tìm thấy role");
            }
            Role = await _roleManager.FindByIdAsync(roleId);
            if (Role != null)
            {
                input = new InputModel
                {
                    Name = Role.Name,
                };
                Claims =await _context.RoleClaims.Where(rc => rc.RoleId == roleId).ToListAsync();
                return Page();
            }

            return NotFound("Không tìm thấy role");
        }
        public async Task<IActionResult> OnPostAsync( string roleId)
        {
            if (roleId == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            Claims = await _context.RoleClaims.Where(rc => rc.RoleId == roleId).ToListAsync();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            role.Name = input.Name;
            var result =  await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên: {input.Name}";
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
