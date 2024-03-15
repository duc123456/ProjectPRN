using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RazorPage.Models;
using RazorPage.Services;
using static RazorPage.Areas.Admin.Pages.IndexModel;

namespace RazorPage.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly MyBlogContext _context;

        private readonly CartService _cartService;

        public IndexModel(ILogger<IndexModel> logger, CartService cartService, MyBlogContext context)
        {
            _cartService = cartService;
            _context = context;
            _logger = logger;
        }
        [BindProperty]
        public List<Order> lsOrder { get;set; }
        public void OnGet()
        {
            dastGet = new Dash();
            var a =_context.Orders.Where(x => x.Status.Equals("Đã nhận hàng")).Sum(x => x.OrderTotal);
            dastGet.doanhthu =(int) _context.Orders.Where(x => x.Status.Equals("Đã nhận hàng")).Sum(x => x.OrderTotal);
            dastGet.numberProduct = _context.Products.Select(x => x.Quantity).Sum();
            dastGet.numbercate = _context.Categories.Count();
            dastGet.numberUser = _context.Users.Count();

            lsOrder = new List<Order>();    
            lsOrder = _context.Orders.ToList();
        }


        public class Dash
        {
            public int doanhthu { get; set; }

            public int numberProduct { get; set; }

            public int numbercate { get; set; }

            public int numberUser{ get; set; }

        }
        [BindProperty]
        public Dash dastGet { get; set; }
        public JsonResult OnPostThongKeDoanhThu(string to, string from)
        {
            Dash dash = new Dash();
            var doanhthu = _context.Orders.Where(x=>x.Status.Equals("Đã nhận hàng"));
            if (from != null)
            {
                doanhthu = doanhthu.Where(x=>x.UpdateDate >= DateTime.Parse(from));
            }
            if (to != null)
            {
                doanhthu = doanhthu.Where(x => x.UpdateDate <= DateTime.Parse(to));
            }
            dash.doanhthu = (int)doanhthu.Sum(x=>x.OrderTotal);
            dash.numberProduct = _context.Products.Select(x=>x.Quantity).Sum();
            dash.numbercate = _context.Categories.Count();
            dash.numberUser = _context.Users.Count();


            return new JsonResult(dash);
        }
        public class ChangeStatus
        {
            public string status { get; set; }
            public int orderId { get; set; }
        }

        public JsonResult OnPostChangeStatus(int orderId)
        {
            ChangeStatus change = new ChangeStatus();

            if (orderId != null)
            {
                Order od = _context.Orders.Find(orderId);
                if(od.Status.Equals("Đã xác nhận")){
                    od.Status = "Đang giao hàng";
                    change.status = "Đang giao hàng";
                }else if(od.Status.Equals("Đang giao hàng"))
                {
                    od.Status = "Đã nhận hàng";
                    change.status = "Đã nhận hàng";
                }
                else if (od.Status.Equals("Đã nhận hàng"))
                {
                    od.Status = "Đã hủy hàng";
                    change.status = "Đã hủy hàng";
                }else if(od.Status.Equals("Đã hủy hàng"))
                {
                    od.Status = "Chờ xác nhận";
                    change.status = "Chờ xác nhận";
                }
                else if (od.Status.Equals("Chờ xác nhận"))
                {
                    od.Status = "Đã xác nhận";
                    change.status = "Đã xác nhận";
                }
                _context.Orders.Update(od);
                _context.SaveChanges();
                change.orderId = orderId;
            }


            return new JsonResult(change);
        }

        public PartialViewResult OnPostGetOrderDetail(int orderId)
        {
            var Orderdtls = _context.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            return new PartialViewResult
            {
                ViewName = "_DetailOrder",
                ViewData = new ViewDataDictionary<List<OrderDetail>>(ViewData, Orderdtls)
            };
        }




    }
}
