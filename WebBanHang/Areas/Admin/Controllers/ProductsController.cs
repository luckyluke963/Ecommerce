using Models;
using Models.Common;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page, string Searchtext)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Create()
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategorys.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {

                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            product.Image = Images[i];
                            product.ProductImage.Add(new ProductImage
                            {
                                ProductId = product.Id,
                                image = Images[i],
                                IsDefaul = true,
                            });
                        }
                        else
                        {
                            product.ProductImage.Add(new ProductImage
                            {
                                ProductId = product.Id,
                                image = Images[i],
                                IsDefaul = false,
                            });
                        }
                    }

                }

                product.CreateDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(product.SeoTitle))
                {
                    product.SeoTitle = product.Title;
                }
                if (string.IsNullOrEmpty(product.Alias))
                {
                    product.Alias = Filterchar.FilterChar(product.Title);
                }
                if (string.IsNullOrEmpty(product.Image))
                {
                    product.Image = "/Content/User/image/hinhloi.png";
                }
               
                product.IsHome = true;
                db.Products.Add(product);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategorys.ToList(), "Id", "Title");
            return View(product);
        }


        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategorys.ToList(), "Id", "Title");
            var item = db.Products.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = Filterchar.FilterChar(model.Title);
              
                db.Products.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                var checkImg = item.ProductImage.Where(x => x.ProductId == item.Id).ToList();
                if (checkImg != null)
                {
                    foreach (var img in checkImg)
                    {
                        db.productImages.Remove(img);
                        db.SaveChanges();
                    }
                }   
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
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
                        var obj = db.Products.Find(Convert.ToInt32(item));
                        db.Products.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = product.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.IsHome = !product.IsHome;
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = product.IsHome });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.IsSale = !product.IsSale;
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsSale = product.IsSale });
            }
            return Json(new { success = false });
        }

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
                    List<Product> list = new List<Product>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        Product product = new Product();
                        product.Title = ((Excel.Range)range.Cells[row, 1]).Text;
                        product.Alias = Filterchar.FilterChar(product.Title);
                        product.ProductCode = ((Excel.Range)range.Cells[row, 2]).Text;
                        product.Description = ((Excel.Range)range.Cells[row, 3]).Text;
                        product.Detail = ((Excel.Range)range.Cells[row, 4]).Text;
                        product.Image = ((Excel.Range)range.Cells[row, 5]).Text;
                        product.Price = decimal.Parse(((Excel.Range)range.Cells[row, 6]).Text);
                        product.PriceSale = decimal.Parse(((Excel.Range)range.Cells[row, 7]).Text);
                        product.Quantity = int.Parse(((Excel.Range)range.Cells[row, 8]).Text);
                        product.SeoTitle = ((Excel.Range)range.Cells[row, 9]).Text;
                        product.SeoDescription = ((Excel.Range)range.Cells[row, 10]).Text;
                        product.SeoKeywords = ((Excel.Range)range.Cells[row, 11]).Text;
                        if (((Excel.Range)range.Cells[row, 11]).Text == "Hiển thị")
                        {
                            product.IsActive = true;
                        }
                        else
                        {
                            product.IsActive = false;
                        }
                        if (((Excel.Range)range.Cells[row, 12]).Text == "Hiển thị")
                        {
                            product.IsActive = true;
                        }
                        else
                        {
                            product.IsActive = false;
                        }
                        if (((Excel.Range)range.Cells[row, 13]).Text == "Hiển thị")
                        {
                            product.IsActive = true;
                        }
                        else
                        {
                            product.IsActive = false;
                        }
                        if (((Excel.Range)range.Cells[row, 14]).Text == "Hiển thị")
                        {
                            product.IsActive = true;
                        }
                        else
                        {
                            product.IsActive = false;
                        }
                        product.CreateDate = DateTime.Now;
                        product.ModifiedDate = DateTime.Now;
                        db.Products.Add(product);
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Index");



            }




        }

    }
    
}