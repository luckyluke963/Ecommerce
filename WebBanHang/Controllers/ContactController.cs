using Microsoft.Office.Interop.Excel;
using Models;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class ContactController : BaseUserController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Contact
        public ActionResult Index()
        {
            var Contact = db.Contacts.Single(x => x.Status == true);
            return View(Contact);
        }

        [HttpPost]
        public JsonResult Send(string name, string mobile, string email, string content)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Phone = mobile;
            feedback.Email = email;
            feedback.Content = content;
            feedback.CreateDate = DateTime.Now;
    
            db.Feedbacks.Add(feedback);
            db.SaveChanges();
            var id = feedback.ID;
            if(id > 0)
            {
                WebBanHang.Common.Common.SendMail("ShopOnline", "phản hồi", feedback.Name, ConfigurationManager.AppSettings["EmailAdmin"]);
                return Json(new
                {
                    status = true
                });
              
            }
            else { return Json(new { status = false });}
        }
    }
}