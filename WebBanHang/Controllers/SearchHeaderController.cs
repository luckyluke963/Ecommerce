using Models;
using Models.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class SearchHeaderController : BaseUserController
    {
        // GET: SearchHeader
        
        public ActionResult SreachBar()
        {
            return PartialView("_SreachBar");
        }


        public JsonResult Search()
        {
            var db = new ApplicationDbContext();
            List<Product> product = db.Products.ToList();
         
            var result = product.Select(p => new
            {
                Id = p.Id,
                Title = p.Title,
                Image = p.Image,
                Price = p.Price,
                PriceSale = p.PriceSale,
                ViewCount = p.ViewCount,
                ProductImage = new List<string>  // Tạo một danh sách chuỗi
                {
                 p.ProductImage.FirstOrDefault(x => x.IsDefaul)?.image  // Thêm strImage vào danh sách (nếu có)
                }
            }).ToList();
            ViewBag.Product = result;
            
            string value = string.Empty;
            value = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            return Json(value,JsonRequestBehavior.AllowGet);
        }
    }
}