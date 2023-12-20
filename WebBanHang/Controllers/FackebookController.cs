using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Office.Interop.Excel;
using Models;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class FackebookController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public FackebookController()
        {
        }

        public FackebookController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult fb()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }


        public long InsertForFacebook(login enty)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == enty.UserName);
            if (user == null)
            {
                db.Users.Add(new ApplicationUser
                {
                    UserName = enty.FullName,
                    Email = enty.UserName,
                    FullName = enty.FullName
                    // Các thuộc tính khác cần được gán tương ứng
                });

                db.SaveChanges();

                db.SaveChanges();
                return enty.id;
            }
            else
            {
                return int.Parse(user.Id);
            }
        }
        //public ActionResult FacebookCallback(string code)
        //{
            
        //    var fb = new FacebookClient();
        //    dynamic result = fb.Post("oauth/access_token", new
        //    {
        //        client_id = ConfigurationManager.AppSettings["FbAppId"],
        //        client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        code = code
        //    });
        //    var accessToken = result.access_token;
        //    if(!string.IsNullOrEmpty(accessToken))
        //    {
        //        fb.AccessToken = accessToken;
        //        dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
        //        string email = me.email;
        //        string userName = me.email;
        //        string firstname = me.first_name;
        //        string middlename = me.middle_name;
        //        string lastname = me.last_name;

        //        var user = new login();
        //        user.Email = email;
        //        user.UserName = userName;
        //        user.FullName = firstname + " " + middlename + " " + lastname;
        //        var aa = InsertForFacebook(user);
        //        if(aa >0)
        //        {

        //        }
        //    }
        //    else
        //    {

        //    }
        //}

       



    }
}