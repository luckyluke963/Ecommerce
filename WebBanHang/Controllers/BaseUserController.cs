using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanHang.Controllers
{
    public class BaseUserController : Controller
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
    }
}