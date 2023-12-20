﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private ApplicationDbContext db =  new ApplicationDbContext();
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                if (user != null)
                {
                    item.Email = user.Email;    
                    item.FullName = user.FullName;
                    item.UserName = user.UserName;
                    item.Avatar = user.image;
                }
                return PartialView(item);
            }

            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult _Load_Review(int productId)
        {
            var item = db.Reviews.Where(x=>x.ProductId == productId).OrderByDescending(x=>x.Id).ToList();
            ViewBag.Count = item.Count;
            return PartialView(item);
        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostReview(ReviewProduct req)
        {
            if(ModelState.IsValid)
            {
                if(req.Avatar == null)
                {
                    req.Avatar = "../picture/avatarnull.png";
                }
                req.CreateDate = DateTime.Now;
                db.Reviews.Add(req);
                db.SaveChanges();
                // Trả về URL để load lại phần tử HTML chứa đánh giá
                var url = Url.Action("_Load_Review", new { productId = req.ProductId });
                return Json(new {Success = true});  
            }
            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {


            base.Dispose(disposing);
        }



        public ActionResult LichSuDonHang()
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                var item = db.Orders.Where(x =>x.CustomerId == user.Id).ToList();
                return PartialView(item);
            }
            return PartialView();
        }



        public ActionResult ViewThongtinDonHang(int id)
        {
          

            var oderDetailId = db.OrderDetails.Find(id);
            return PartialView(oderDetailId);
        }
    }
}