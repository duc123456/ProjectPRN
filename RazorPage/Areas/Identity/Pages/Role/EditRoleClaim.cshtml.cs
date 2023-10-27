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
    public class EditRoleClaimModel : RolePageModel
    {
        public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext context) : base(roleManager, context)
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
        IdentityRoleClaim<string> claim { get; set; }
        public async Task<IActionResult> OnGet(int claimid)
        {
            if (claimid == null) return NotFound("Không tìm thấy role");
            claim =  _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null) return NotFound("Không tìm thấy claim");
            Role =  await _roleManager.FindByIdAsync(claim.RoleId);
            if (Role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            input = new InputModel()
            {
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
            };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int claimid)
        {
            if (claimid == null) return NotFound("Không tìm thấy role");
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null) return NotFound("Không tìm thấy claim");
            Role =await _roleManager.FindByIdAsync(claim.RoleId);
            if (Role == null) return NotFound("Không tìm thấy role");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(_context.RoleClaims.Any(c=>c.RoleId == Role.Id && c.ClaimType== input.ClaimType && c.ClaimValue == input.ClaimValue && c.Id== claim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
                return Page();
            }
            claim.ClaimType = input.ClaimType;
            claim.ClaimValue = input.ClaimValue;
            await _context.SaveChangesAsync();
            StatusMessage = "Vừa cập nhật Claim";



            return RedirectToPage("./Edit", new {roleid = Role.Id});
        }
        public async Task<IActionResult> OnPostDeleteAsync(int claimid)
        {
            if (claimid == null) return NotFound("Không tìm thấy role");
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null) return NotFound("Không tìm thấy claim");
            Role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (Role == null) return NotFound("Không tìm thấy role");
          
            await _roleManager.RemoveClaimAsync(Role, new Claim(claim.ClaimType, claim.ClaimValue));

            StatusMessage = "Vừa cập xóa Claim";



            return RedirectToPage("./Edit", new { roleid = Role.Id });
        }
    }
}
