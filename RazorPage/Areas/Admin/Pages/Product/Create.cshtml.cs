﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Areas.Admin.Pages.Product
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : PageModel
	{
		private readonly MyBlogContext _context;
		private readonly IWebHostEnvironment _environment;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		public CreateModel(MyBlogContext context, IWebHostEnvironment environment, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_context = context;
			_environment = environment;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[BindProperty]

		public RazorPage.Models.Product product { get; set; }

		[TempData]
		public string StatusMessage { get; set; }


		public List<SelectListItem> Categories { get; set; }
		public void OnGet()
		{
			Categories = _context.Categories.Select(
			n => new SelectListItem
			{
				Value = n.CategoryId.ToString(),
				Text = n.CategoryName.ToString(),
			}).ToList();
		}
		public async Task<IActionResult> OnPostAsync(List<string> Images, int radioDefault)
		{

			if (!ModelState.IsValid)
			{
				Categories = _context.Categories.Select(
				n => new SelectListItem
				{
					Value = n.CategoryId.ToString(),
					Text = n.CategoryName.ToString(),
				}).ToList();
				return Page();
			}
			else
			{
				if (Images != null && Images.Count > 0)
				{
					for (int i = 0; i < Images.Count; i++)
					{
						if (i + 1 == radioDefault)
						{
							product.ImageDefault = Images[i];
							product.ProductImages.Add(new ProductImage
							{
								ProductId = product.ProductId,
								Image = Images[i],
								IsDefault = true,
								CreatedDate = DateTime.Now,
							});
						}
						else
						{
							product.ProductImages.Add(new ProductImage
							{
								ProductId = product.ProductId,
								Image = Images[i],
								IsDefault = false,
								CreatedDate = DateTime.Now,

							});
						}

					}
				}
				var user = await _userManager.GetUserAsync(User);
				product.CreateBy = user.UserName;
				product.IsDeleted = false;
				product.UpdateDate = DateTime.Now;
				product.CreateDate = DateTime.Now;
				_context.Products.Add(product);
				_context.SaveChanges();
				StatusMessage = "Bạn vừa tạo thành công sản phẩm" + product.ProductName;

			}
			return RedirectToPage("./Index");

		}
	}
}
