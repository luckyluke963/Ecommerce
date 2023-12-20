using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.EF;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class HomeController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private  ApplicationUserManager _userManager;

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        
        }
        public HomeController() { }



        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin/Home
        public async Task<ActionResult> Index()
        {
            ViewBag.countOrder = db.Orders.Count(i =>i.Status ==  1);
            ViewBag.countProduct = db.Products.Count();

            var customerRole = db.Roles.FirstOrDefault(y => y.Name == "customer");
            var countCustomers = db.Users.Count(x => x.Roles.Any(r => r.RoleId == customerRole.Id));
            ViewBag.CountCustomer = countCustomers;

            if(!string.IsNullOrEmpty(User.Identity.Name))
            {



                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                var itemss = new UserAdminPagingIndex();
                var dd = user.Roles.FirstOrDefault().RoleId;
                var bb = db.Roles.Where(x => x.Id == dd);
               
                
                    var Nameroless = bb.FirstOrDefault().Name;
                ViewBag.AA = Nameroless;
                
                itemss.FullName = user.FullName;
                //var idrole = db.Roles.FirstOrDefault(x=>x.Id == int.Parse(user.Id));
                //itemss.RoleName = idrole.Name;

                ViewBag.TenQuanTri = itemss.FullName;
               
            }

            var item = new ThongKeModel();
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];

            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreateDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}