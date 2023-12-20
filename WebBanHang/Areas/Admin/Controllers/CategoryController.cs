
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.EF;
using Models.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Common;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseAdminController
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public CategoryController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }
        public CategoryController() { }



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













        // GET: Admin/Category
        public async Task<ActionResult> Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Category> item = _dbContext.Categories.OrderByDescending(x => x.Id);
            //var item = _dbContext.Categories;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            item = item.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;





            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var itemss = new UserAdminPagingIndex();
            var dd = user.Roles.FirstOrDefault().RoleId;
            var bb = _dbContext.Roles.Where(x => x.Id == dd);
            var Nameroless = bb.FirstOrDefault().Name;
            ViewBag.AA = Nameroless;





            return View(item);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentCulture = Session[CommonConstants.CurrentCulture];
                    category.Language = currentCulture.ToString();
                    category.CreateDate = DateTime.Now;
                    category.ModifiedDate = DateTime.Now;
                 
                    category.Alias = Filterchar.FilterChar(category.Title);
                    _dbContext.Categories.Add(category);
                    _dbContext.SaveChanges();
                    SetAlert(StaticRescouces.Resources.themdanhmucthanhcon, StaticRescouces.Resources.thanhcong);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", StaticRescouces.Resources.themdanhmuckhongthanhcong);
                }

            }
            return View(category);
        }


        public ActionResult Edit(int id)
        {
            var item = _dbContext.Categories.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Categories.Attach(category);
                    category.ModifiedDate = DateTime.Now;
                    category.Alias = Filterchar.FilterChar(category.Title);
                    _dbContext.Entry(category).Property(c => c.Title).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.Description).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.Alias).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.SeoDescription).IsModified = true;

                    _dbContext.Entry(category).Property(c => c.SeoKeywords).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.SeoTitle).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.Position).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.ModifiedDate).IsModified = true;
                    _dbContext.Entry(category).Property(c => c.ModifiedBy).IsModified = true;

                    _dbContext.SaveChanges();
                    SetAlert("Sửa danh mục thành công", "success");
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Sửa danh mục không thành công");
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbContext.Categories.Find(id);
            if (item != null)
            {
               
                    //var DeleteItem = _dbContext.Categories.Attach(item);
                    _dbContext.Categories.Remove(item);
                    _dbContext.SaveChanges();
                  
                    return Json(new { success = true });
               
                

                
            }
            return Json(new { success = false });
        }
    }
}