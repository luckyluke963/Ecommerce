    using Models;
using Models.EF;
using Models.Model;
using PagedList;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class ProductsController : BaseUserController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(int? id,int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;

              
            }
            if(pageSize == null)
            {
                pageSize = 12;
            }
            var item = db.Products.ToList();
            if (id != null)
            {
                item = item.Where(x => x.ProductCategoryId == id).ToList();
            }
            
            ViewBag.pageSize = pageSize;

            return View(item.ToPagedList((int)page,(int)pageSize));
        }







        public ActionResult Detail(string alias ,int id)
        {
            var item = db.Products.Find(id);
            if(item!=null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x=>x.ViewCount).IsModified = true;
                db.SaveChanges();
            }




            var list = Session["RecentProductList"] as List<RecentProduct>;
            if (list == null)
            {
                // If list not found in session, create list and store it in a session
                list = new List<RecentProduct>();
                Session["RecentProductList"] = list;
            }

            // Add product to recent list (make list contain max 10 items; change if you like)
            AddRecentProduct(list, id, item.Title,item.Image,item.Price,item.PriceSale,item.Alias , 3);

            // Store recentProductList to ViewData keyed as "RecentProductList" to use it in a view
            TempData["RecentProductList"] = list;

            var countReview = db.Reviews.Where(x => x.ProductId == id).Count();
            ViewBag.CountReview = countReview;  
            
            return View(item);
        }







        public void AddRecentProduct(List<RecentProduct> list, int id,string name, string image,decimal price, decimal? pricesale, string alasa, int maxItems)
        {
            // list is current list of RecentProduct objects
            // Check if item already exists
            var item = list.FirstOrDefault(t => t.ProductId == id);
            // TODO: here if item is found, you could do some more coding
            //       to move item to the end of the list, since this is the
            //       last product referenced.
            if (item == null)
            {
                // Add item only if it does not exist
                list.Add(new RecentProduct
                {
                    ProductId = id,
                    ProdutName = name,
                    image = image,
                    price = price,
                    priceSale = pricesale,
                    alas = alasa,
                    LastVisited = DateTime.Now,
                });
            }

            // Check that recent product list does not exceed maxItems
            // (items at the top of the list are removed on FIFO basis;
            // first in, first out).
            while (list.Count > maxItems)
            {
                list.RemoveAt(0);
            }
        }












        public ActionResult ProductCategory(string alias,int? id)
        {
            var item = db.Products.ToList();
            if (id > 0)
            {
                item = item.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate=  db.ProductCategorys.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(item);
        }


        public ActionResult Partial_ItemByCateId()
        {
            var items = db.Products.Where(x=>x.IsHome && x.IsActive).Take(8).ToList();
            return PartialView(items);
        }

        public ActionResult Paritail_Relatedproducts(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                int cateId = item.ProductCategoryId;
                var relatedProducts = db.Products
                    .Where(x => x.ProductCategoryId == cateId && x.Id != id) // Lấy sản phẩm cùng danh mục và loại trừ sản phẩm đang xem
                    .Take(14) // Giới hạn số lượng sản phẩm
                    .ToList();

                return PartialView(relatedProducts);
            }

            // Trả về PartialView rỗng nếu không tìm thấy sản phẩm được xem chi tiết
            return PartialView(new List<Product>());
        }


        public ActionResult Partial_ProductSales()
        {
            var items = db.Products
      .Where(x => x.IsSale && x.IsActive && x.Price > 0 && ((x.Price - x.PriceSale) / x.Price * 100) < 10)
      .Take(8)
      .ToList();



            return PartialView(items);

        }

        [ChildActionOnly]
        public PartialViewResult PartialProductCategory()
        {
            var items = db.ProductCategorys.Where(x=>x.Status==true).OrderBy(x=>x.Title).ToList();
            return PartialView(items);
        }


       
       
    }
}