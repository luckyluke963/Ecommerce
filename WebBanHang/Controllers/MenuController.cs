using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class MenuController : BaseUserController
    {
        private ApplicationDbContext db = new ApplicationDbContext();   
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
        {
            var item = db.Categories.OrderBy(x=>x.Position).ToList();
            return PartialView("_MenuTop",item);
        }

        public ActionResult MenuProductCategory()
        {
            var item = db.ProductCategorys.Where(x => x.Status == true).OrderBy(x => x.Title).ToList();
          
            return PartialView("_MenuProductCategory", item);
        }

        public ActionResult MenuArraival()
        {
            var item = db.ProductCategorys.Where(x => x.Status == true).OrderBy(x => x.Title).ToList();
            return PartialView("_MenuArraival", item);
        }


        public ActionResult MenuLeft(int? id)
        {
            if(id != null)
            {
                ViewBag.CateId = id;
            }
            var item = db.ProductCategorys.Where(x => x.Status == true).OrderBy(x => x.Title).ToList();
            return PartialView("_MenuLeft", item);
        }


        public ActionResult SearchHeader()
        {
           
            return PartialView("_Search");
        }

    }
}