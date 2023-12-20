using Models;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();   
        // GET: Admin/ProductImages
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = db.productImages.Where(x=>x.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId,string url)
        {
            Product product = db.Products.Find(productId);
            // Add a new ProductImage
            // Check if the product already has an image
            bool hasDefaultImage = product.Image != null;

            // Add a new ProductImage
            ProductImage newImage = new ProductImage
            {
                ProductId = productId,
                image = url,
                IsDefaul = !hasDefaultImage // Set to true if there is no default image yet
            };

            db.productImages.Add(newImage);
            db.SaveChanges();

            // Set the product's Image property to the newly added image URL
            product.Image = newImage.image;
           
            db.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.productImages.Find(id);
            db.productImages.Remove(item);
            db.SaveChanges();
            return Json(new {success = true});

        }
    }
}