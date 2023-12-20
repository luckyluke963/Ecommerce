using Models;
using Models.Common;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class SlideController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Slide
        public ActionResult Index()
        {
            var item = db.Slides.ToList();
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slide model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
              
                model.ModifiedDate = DateTime.Now;
               
                db.Slides.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var item = db.Slides.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slide model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
              
                db.Slides.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = db.Slides.Find(id);
            if (item != null)
            {
                db.Slides.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]

        public ActionResult IsActive(int id)
        {
            var item = db.Slides.Find(id);
            if (item != null)
            {
                item.Status = !item.Status;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.Status });
            }
            return Json(new { success = false });
        }

    }
}