// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace App.Admin.User

{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public AppUser user { get; set; }
        [BindProperty]
        [DisplayName("Các role gắn cho user")]
        public string[] RoleNames { get; set; }

        public SelectList allRole { get; set; }
       

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user =await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray();

            List<string> roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRole = new SelectList(roles);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound($"Không tìm thấy user");
            }
           //Roles Name
           var oldRoleName = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRole = oldRoleName.Where(r => !RoleNames.Contains(r));
            var addRoles= RoleNames.Where(r=>!oldRoleName.Contains(r));

            List<string> roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRole = new SelectList(roles);
            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRole);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
            StatusMessage = $"Vừa cập nhật role cho user {user.UserName}";
            return RedirectToPage("./Index");
        }
    }
}
