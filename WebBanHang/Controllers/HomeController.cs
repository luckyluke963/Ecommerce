using Microsoft.Office.Interop.Excel;
using Models;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{

    public class HomeController : BaseUserController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var item = db.Slides.Where(x=>x.Status == true).OrderBy(x =>x.DisplayOrder).ToList();
            ViewBag.Slides = item;
            ViewBag.NewProducts = ListNewProduct(8);
            ViewBag.ListFeatureProducts = ListFeatureProduct(8);
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult Refresh()
        {
            var item = new ThongKeModel();
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            item.HomNay = HttpContext.Application["HomNay"].ToString();
            item.HomQua = HttpContext.Application["HomQua"].ToString();
            item.TuanNay = HttpContext.Application["TuanNay"].ToString();
            item.TuanTruoc = HttpContext.Application["TuanTruoc"].ToString();
            item.ThangNay = HttpContext.Application["ThangNay"].ToString();
            item.ThangTruoc = HttpContext.Application["ThangTruoc"].ToString();
            item.TatCa = HttpContext.Application["TatCa"].ToString();
            return PartialView(item);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            //return db.Products.Where(x => x.IsHot && x.ViewCount >= 100).OrderByDescending(x => x.CreateDate).Take(top).ToList();
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Lấy các sản phẩm có số lượng mua lớn hơn 0, là sản phẩm bán chạy và được tạo trong tháng hiện tại
            return db.Products
                .Where(x => x.IsHot &&  x.Warranty > 0)
                .OrderByDescending(x => x.CreateDate)
                .Take(top)
                .ToList();
        }


        public ActionResult Partial_subcrice()
        {
            return PartialView();
        }

        [HttpPost]
     
        public ActionResult Subcrice(Subscribe req)
        {
            if (req.Email == null)
            {
                return Json(new { Rong = false });
            }
            else
            if (ModelState.IsValid)
            {
             
                if (db.Subscribes.Any(x => x.Email == req.Email))
                {
                    return Json(new { Sai = false });
                }
                

                string randomCoupon = GenerateRandomCoupon();
                DateTime expirationTime = DateTime.Now; // Set expiration time to 7 days from now

                db.Subscribes.Add(new Subscribe { Email = req.Email, CreateTime = expirationTime, MaCode = randomCoupon , status =true });
                db.SaveChanges();

                // Replace placeholders in the email template
                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/layoutsendemail/CouponCodeWebDesign.html"));
                contentCustomer = contentCustomer.Replace("{{MaCode}}", randomCoupon);
                contentCustomer = contentCustomer.Replace("{{Ngay}}", expirationTime.AddDays(7).ToString("dd/MM/yyyy"));

                // Send the email
                WebBanHang.Common.Common.SendMail("Shop DV Linh kiện PC", "Phiếu giảm giá", contentCustomer, req.Email);

                return Json(new { Success = true });
            }

            return View("Partial_subcrice", req);
        }

        private string GenerateRandomCoupon()
        {
            // Chuỗi ký tự có thể sử dụng để tạo mã ngẫu nhiên
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Độ dài của mã khuyến mãi
            const int couponLength = 8;

            // Sử dụng Random để tạo ngẫu nhiên
            Random random = new Random();

            // Tạo mã ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi characters
            char[] couponChars = new char[couponLength];
            for (int i = 0; i < couponLength; i++)
            {
                couponChars[i] = characters[random.Next(characters.Length)];
            }

            // Trả về mã khuyến mãi dưới dạng chuỗi
            return new string(couponChars);
        }
        public ActionResult ProductSell()
        {
            return View();
        }
    }
}