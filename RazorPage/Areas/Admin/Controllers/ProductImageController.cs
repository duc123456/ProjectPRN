using Microsoft.AspNetCore.Mvc;
using RazorPage.Models;
using System;

namespace RazorPage.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    public class ProductImageController : Controller
    {
        private readonly MyBlogContext _context;
        public ProductImageController(MyBlogContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { Success = true, Message = "Image added successfully" });
        }
        [HttpPost]
        public IActionResult Post(int id, string url)
        {
            try
            {
                _context.ProductImages.Add(new ProductImage
                {
                    ProductId = id,
                    Image = url,
                    IsDefault = false
                });
                _context.SaveChanges();
                return Json(new { Success = true, Message = "Image added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Fuck()
        {
            return Json(new { Success = true, Message = "Image added successfully" });

        }

    }
}
