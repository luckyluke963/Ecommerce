using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Threading;
using System.Web.Routing;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session[CommonConstants.CurrentCulture] != null)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Session[CommonConstants.CurrentCulture].ToString());


            }
            else
            {

                Session[CommonConstants.CurrentCulture] = "vi";
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("vi");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi");

            }
        }

        public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(ddlCulture);

            Session[CommonConstants.CurrentCulture] = ddlCulture;
            return Redirect(returnUrl);

        }


        protected void SetAlert(string message, string type)
        {
            TempData["AlertMesssage"] = message;
            if (type == "success")

            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warring")
            {
                TempData["AlertType"] = "alert-warring";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

        protected override void Dispose(bool disposing)
        {
            

            base.Dispose(disposing);
        }



    }
}