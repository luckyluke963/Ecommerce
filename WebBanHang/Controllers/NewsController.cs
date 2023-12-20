using Microsoft.Office.Interop.Excel;
using Models;
using Models.EF;
using Models.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class NewsController : BaseUserController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: News
        public ActionResult Index(int? page, string name)
        {
            var pageSize = 4;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = db.News.OrderByDescending(x => x.CreateDate).Where(x=>x.IsActive == true);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            if (!string.IsNullOrEmpty(name))
            {
                items = items.Where(x => x.Title.Contains(name));
            }

          

            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.LastNew = LastNew(3);

            var recentProductList = TempData["RecentProductList"] as List<RecentProduct>;
            
            if (recentProductList != null)
            {
                // Do something with recentProductList in NewsController
                // For example, pass it to the view
                ViewData["RecentProductList"] = recentProductList;
            }


           var  listProduct = db.ProductCategorys.Where(x => x.Status == true).OrderBy(x => x.Title).ToList();

            ViewBag.listProduct = listProduct;




           




            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }


        


        public ActionResult Detail(int id)
        {
            var item = db.News.Find(id);

            ViewBag.LastNew = LastNew(3);



            var list = Session["RecentNewsList"] as List<RecentNews>;
            if (list == null)
            {
                // If list not found in session, create list and store it in a session
                list = new List<RecentNews>();
                Session["RecentNewsList"] = list;
            }

            // Add product to recent list (make list contain max 10 items; change if you like)
            AddRecentNews(list, id, item.Title, item.Image,  item.Alias, 3);

            // Store recentProductList to ViewData keyed as "RecentProductList" to use it in a view
            TempData["RecentNewsList"] = list;













            var listProduct = db.ProductCategorys.Where(x => x.Status == true).OrderBy(x => x.Title).ToList();

            ViewBag.listProduct = listProduct;
            return View(item);
        }


        public List<News> LastNew (int top)
        {
           
            return db.News.OrderByDescending(x=>x.CreateDate).Take(top).ToList();
        }



        public void AddRecentNews(List<RecentNews> list, int id, string name, string image, string alasa, int maxItems)
        {
            // list is current list of RecentProduct objects
            // Check if item already exists
            var item = list.FirstOrDefault(t => t.NewsId == id);
            // TODO: here if item is found, you could do some more coding
            //       to move item to the end of the list, since this is the
            //       last product referenced.
            if (item == null)
            {
                // Add item only if it does not exist
                list.Add(new RecentNews
                {
                    NewsId = id,
                    NewsName = name,
                    image = image,
                    
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








        public ActionResult Partial_News_Home()
        {
            var item = db.News.Take(3).ToList();
            return PartialView(item);
        }
    }
}