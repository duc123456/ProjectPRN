using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Admin.Role
{
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, MyBlogContext context) : base(roleManager, context)
        {
        }

        public class InputModel
        {
            [Display(Name = "Tên của role")]
            [Required(ErrorMessage ="Phải nhâp {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage ="{0} phải dài từ {2} tới {1} kí tự")]
            public string Name { get; set; }
        }


        [BindProperty]
        public  InputModel input { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var newRole = new IdentityRole(input.Name);
            var result= await _roleManager.CreateAsync(newRole);
            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo thành công role mới: {input.Name}";
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
