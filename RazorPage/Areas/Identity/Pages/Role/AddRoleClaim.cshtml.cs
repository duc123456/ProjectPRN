using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace App.Admin.Role
{
    public class AddRoleClaimModel : RolePageModel
    {
        public AddRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext context) : base(roleManager, context)
        {
        }

        public class InputModel
        {
            [Display(Name = "Kiểu Claim")]
            [Required(ErrorMessage ="Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage ="{0} phải dài từ {2} tới {1} kí tự")]
            public string ClaimType { get; set; }

            [Display(Name = "Giá trị")]
            [Required(ErrorMessage = "Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} tới {1} kí tự")]
            public string ClaimValue { get; set; }
        }
        public IdentityRole Role { get; set; }


        [BindProperty]
        public  InputModel input { get; set; }
        public async Task<IActionResult> OnGet(string roleid)
        {
            Role =  await _roleManager.FindByIdAsync(roleid);
            if (Role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            Role =await _roleManager.FindByIdAsync(roleid);
            if (Role == null) return NotFound("Không tìm thấy role");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if((await _roleManager.GetClaimsAsync(Role)).Any(c=>c.Type == input.ClaimType && c.Value == input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
                return Page();
            }
            var newClaim = new Claim(input.ClaimType, input.ClaimValue);
            var result = await _roleManager.AddClaimAsync(Role, newClaim);
            if(!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                
            }
            StatusMessage = "Vừa thêm đặc tính mới của Claim";


            return Page();
        }
    }
}
