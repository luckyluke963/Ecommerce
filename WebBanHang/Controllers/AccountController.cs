using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Xml.Linq;
using BotDetect.Web.Mvc;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Office.Interop.Excel;
using Microsoft.Owin.Security;
using Models;
using Models.EF;
using WebBanHang.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace WebBanHang.Controllers
{
    [Authorize]
    public class AccountController : BaseUserController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", StaticRescouces.ResourceUser.taikhoanhoacmatkhaukhonghople);
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "exampleCaptcha", "mã xác nhận không đúng!")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Address = model.Address,
                    CreatetAcount = DateTime.Now,
                    image = "../picture/avatarnull.png",
                };
                if (!string.IsNullOrEmpty(model.ProviceID))
                {
                    user.ProvinceID = int.Parse(model.ProviceID);
                }
                if (!string.IsNullOrEmpty(model.DistrictID))
                {
                    user.DistrictID = int.Parse(model.DistrictID);
                }
                if (!string.IsNullOrEmpty(model.WardID))
                {
                    user.WardID = int.Parse(model.WardID);
                }
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    UserManager.AddToRole(user.Id, "Customer");
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                var it = (await UserManager.IsEmailConfirmedAsync(user.Id));
                if (user == null )
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/layoutsendemail/sendEmailForgetPass.html"));
                contentCustomer = contentCustomer.Replace("{{reset}}", callbackUrl);
                WebBanHang.Common.Common.SendMail("ShopOnline", "Quên mật khẩu", contentCustomer.ToString(), model.Email);
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                 return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


        public async Task<ActionResult> Profile()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var item = new CreateAccountViewModel();
            item.Email = user.Email;
            item.FullName = user.FullName;
            item.Phone = user.Phone;
            item.UserName = user.UserName;
            item.Address = user.Address;
            item.ProviceID = user.ProvinceID;
            item.DistrictID = user.DistrictID;
            item.WardID = user.WardID;
            item.CreatetAcount = user.CreatetAcount;
            item.Image = user.image;
         
            string xmlFilePath = Server.MapPath(@"~/Content/data/Provinces_Data.xml"); // Thay đổi đường dẫn đến file XML của bạn
            XDocument doc = XDocument.Load(xmlFilePath);

            // Tìm giá trị của Province dựa trên ProvinceID
            var provinceValue = doc.Descendants("Item").Where(e =>e.Attribute("id") != null && int.TryParse(e.Attribute("id").Value, out int id) && // Kiểm tra xem giá trị có phải số nguyên không
                    id == item.ProviceID &&
                    (string)e.Attribute("type") == "province"
                )
                .Select(e => (string)e.Attribute("value"))
                .FirstOrDefault();

            var districtValue = doc.Descendants("Item")
             .Where(e =>
            e.Attribute("id") != null &&
            int.TryParse(e.Attribute("id").Value, out int id) && // Kiểm tra xem giá trị có phải số nguyên không
            id == item.DistrictID &&
            (string)e.Attribute("type") == "district"
            )
        .Select(e => (string)e.Attribute("value"))
        .FirstOrDefault();

            var WardValue = doc.Descendants("Item")
           .Where(e =>
          e.Attribute("id") != null &&
          int.TryParse(e.Attribute("id").Value, out int id) && // Kiểm tra xem giá trị có phải số nguyên không
          id == item.WardID &&
          (string)e.Attribute("type") == "precinct"
          )
      .Select(e => (string)e.Attribute("value"))
      .FirstOrDefault();



            ViewBag.ProvinceName = provinceValue;
            ViewBag.DistrictName = districtValue;
            ViewBag.WardName = WardValue;



            ViewBag.ProvinceList = doc.Descendants("Item")
    .Where(e => (string)e.Attribute("type") == "province")
    .Select(e => new SelectListItem
    {
        Text = (string)e.Attribute("value"),
        Value = ((int)e.Attribute("id")).ToString()
    })
    .ToList();



            var districtValue1 = doc.Descendants("Item")
     .Where(e =>
         e.Attribute("id") != null &&
         int.TryParse(e.Attribute("id").Value, out int id) && // Kiểm tra xem giá trị có phải số nguyên không
         id == item.DistrictID &&
         (string)e.Attribute("type") == "district"
     )
     .Select(e => new SelectListItem
     {
         Text = (string)e.Attribute("value"),
         Value = ((int)e.Attribute("id")).ToString()
     })
     .FirstOrDefault();

            ViewBag.DistrictList = new List<SelectListItem> { districtValue1 };





            var WardValue1 = doc.Descendants("Item")
    .Where(e =>
        e.Attribute("id") != null &&
        int.TryParse(e.Attribute("id").Value, out int id) && // Kiểm tra xem giá trị có phải số nguyên không
        id == item.WardID &&
        (string)e.Attribute("type") == "precinct"
    )
    .Select(e => new SelectListItem
    {
        Text = (string)e.Attribute("value"),
        Value = ((int)e.Attribute("id")).ToString()
    })
    .FirstOrDefault();

            ViewBag.WardList = new List<SelectListItem> { WardValue1 };












            return View(item);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> UploadImage(CreateAccountViewModel req)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByEmailAsync(req.Email);
        //        user.image = req.Image;
        //        var result = await UserManager.UpdateAsync(user);

        //        if (result.Succeeded)
        //        {
        //            TempData["success"] = "Bạn thay đổi thành công";
        //            return Json(new { Success = true });
        //        }
        //    }

        //    // If there are validation errors or the update failed, return an error response.
        //    return Json(new { Success = false, Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        //}




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostProfile(CreateAccountViewModel req)
        {
            var user = await UserManager.FindByEmailAsync(req.Email);

            if(req.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(req.ImageFile.FileName);
                string extension = Path.GetExtension(req.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                req.Image = "../picture/" + fileName;
                fileName = Path.Combine(Server.MapPath("../picture/"), fileName);
                req.ImageFile.SaveAs(fileName);
            }
            
            

            user.FullName = req.FullName;   
            user.Phone = req.Phone;
            user.image = req.Image;
            user.Address = req.Address;
            if (req.ProviceID.HasValue)
            {
                user.ProvinceID = req.ProviceID;
            }
            if (req.DistrictID.HasValue)
            {
                user.DistrictID = req.DistrictID;
            }
            if (req.WardID.HasValue)
            {
                user.WardID = req.WardID;
            }



            if (!string.IsNullOrEmpty(req.Password))
            {
                var passwordHasher = new PasswordHasher();
                PasswordVerificationResult passwordResult = passwordHasher.VerifyHashedPassword(user.PasswordHash, req.Password);
                if (passwordResult == PasswordVerificationResult.Success)
                {
                    var passwordChangeResult = await UserManager.ChangePasswordAsync(user.Id, req.Password, req.ConfirmPassword);
                    if(req.NewPassword == req.ConfirmPassword)
                    {
                        if (!passwordChangeResult.Succeeded)
                        {

                            TempData["message"] = "Mật khẩu mới và nhập lại mật khẩu không khớp";
                            return RedirectToAction("Profile");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Mật khẩu mới và nhập lại mật khẩu không khớp";
                        return RedirectToAction("Profile");
                    }
                }
                else
                {
                    TempData["message"] = "Bạn sai tài khoản";
                    return RedirectToAction("Profile");
                }
                
            }





            var rs = await UserManager.UpdateAsync(user);
            if(rs.Succeeded)
            {
                TempData["success"] = "Bạn thay đổi thành công";
                return RedirectToAction("Profile");
            }
            
            return View(rs);
           
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
      
        public ActionResult FaceBook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope= "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }





    }
}