using Models;
using Models.Common;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
using Microsoft.AspNet.Identity;
using Models.Model;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class NewsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;



        public NewsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }
        public NewsController() { }


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


        // GET: Admin/News
        public async Task<ActionResult> Index(string Searchtext, int? page)
        {
            var pageSize = 7;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = db.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var itemss = new UserAdminPagingIndex();
            var dd = user.Roles.FirstOrDefault().RoleId;
            var bb = db.Roles.Where(x => x.Id == dd);
            var Nameroless = bb.FirstOrDefault().Name;
            ViewBag.AA = Nameroless;


            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;

                model.ModifiedDate = DateTime.Now;
                model.Alias = Filterchar.FilterChar(model.Title);
                db.News.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }




        public ActionResult Edit(int id)
        {
            var item = db.News.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {

                model.ModifiedDate = DateTime.Now;
                model.CreateDate = DateTime.Now;

                model.Alias = Filterchar.FilterChar(model.Title);
                db.News.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = db.News.Find(id);
            if (item != null) {
                db.News.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]

        public ActionResult IsActive(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.News.Find(Convert.ToInt32(item));
                        db.News.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }



        //[HttpPost]
        //public ActionResult Inport(HttpPostedFileBase excelfile)
        //{
        //    if(excelfile == null|| excelfile.ContentLength == 0)
        //    {
        //        ViewBag.Error = "Please select a excel file</br>";
        //        return View("Index");
        //    }
        //    else
        //    {
        //        if(excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
        //        {
        //            string path = Server.MapPath("~/Content/"+excelfile.FileName);
        //            if(System.IO.File.Exists(path))
        //            {
        //                System.IO.File.Delete(path);
        //            }
        //            excelfile.SaveAs(path);
        //            Excel.Application application = new Excel.Application();
        //            Excel.Workbook workbook = application.Workbooks.Open(path);
        //            Excel.Worksheet worksheet = workbook.ActiveSheet;
        //            Excel.Range range = worksheet.UsedRange;
        //            //List<News> listNew = new List<News>();
        //            for (int row = 3; row < range.Rows.Count; row++)
        //            {
        //                News news = new News();
        //                news.Title = ((Excel.Range)range.Cells[row, 1]).Text;
        //                news.Alias = ((Excel.Range)range.Cells[row, 2]).Text;
        //                news.Description = ((Excel.Range)range.Cells[row, 3]).Text;
        //                news.Image = ((Excel.Range)range.Cells[row, 4]).Text;

        //                news.CateogoryId = 2;
        //                news.CreateDate = DateTime.Now;
        //                news.ModifiedDate = DateTime.Now;
        //                db.News.Add(news);
        //                db.SaveChanges();
        //            }
        //            //ViewBag.listNews = listNew;
        //            return RedirectToAction("Index");
        //        }
        //        else{
        //            ViewBag.Error = "File type is incorrect </br>";
        //            return View("Index");
        //        }
        //    }
        //}
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select an Excel file.</br>";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/Import/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    excelfile.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet workSheet = workbook.ActiveSheet;
                    Excel.Range range = workSheet.UsedRange;
                    List<News> list = new List<News>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        News news = new News();
                        news.Title = ((Excel.Range)range.Cells[row, 1]).Text;
                        news.Alias = Filterchar.FilterChar(news.Title);
                        news.Detail = ((Excel.Range)range.Cells[row, 2]).Text;
                        news.Image = ((Excel.Range)range.Cells[row, 3]).Text;
                        news.SeoTitle  = ((Excel.Range)range.Cells[row, 4]).Text;
                        news.SeoDescription = ((Excel.Range)range.Cells[row, 5]).Text;
                        news.SeoKeywords = ((Excel.Range)range.Cells[row, 6]).Text;
                        if(((Excel.Range)range.Cells[row, 7]).Text == "Hiển thị")
                        {
                            news.IsActive = true;
                        }
                        else
                        {
                            news.IsActive = false;
                        }
                        news.CreateDate = DateTime.Now;
                        news.ModifiedDate = DateTime.Now;
                        db.News.Add(news);
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Index");

            
               
            }
            
        }

    }
}