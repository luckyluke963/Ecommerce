using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Models;
using Models.EF;
using Models.Paymmnetvnpay;
using Models.ShopCart;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PayPal.Api;
using Order = Models.EF.Order;
using Newtonsoft.Json;
using System.Xml.Linq;
using BotDetect;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Threading.Tasks;

namespace WebBanHang.Controllers
{
    [Authorize]
    public class ShoppingCartController : BaseUserController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ShoppingCartController()
        {
        }

        public ShoppingCartController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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



        // GET: ShoppingCart
        [AllowAnonymous] // không cho phép đăng nhập
        public ActionResult CartView()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
                return View(cart.Items);
            }
            return PartialView();
        }




        // GET: ShoppingCart
        [AllowAnonymous] // không cho phép đăng nhập
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
                decimal tongValue = cart.Tong;
                ViewBag.CountDem = cart.count;
                //ViewBag.Tong = tongValue;
                if(cart.count == 0)
                {
                    if (cart.Items.Count == 2)
                    {
                        cart.TongAll = cart.Tong;
                    }
                }
                
                if (cart.TongAll != 0)
                {
                    ViewBag.TongAll = cart.TongAll;
                  
                }
                else 
                {
                    ViewBag.TongAll = tongValue;
                }

                Session["Cart"] = cart;
                //ViewBag.TienAll = cart.Items.Select(x => x.AllPrice);
                return View(cart.Items);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult NhapGiftCode(string MaCode)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                DateTime now = DateTime.Now;
                Subscribe foundSubscribe = db.Subscribes.FirstOrDefault(x => x.MaCode == MaCode);




                if (MaCode == "")
                {
                    return Json(new { Success = false });
                }
                if (cart.count == 1)
                {
                    return Json(new { xx = false });
                }
                if(foundSubscribe != null)
                {
                    if ((now - foundSubscribe.CreateTime).TotalDays > 7)
                    {
                        return Json(new { ss = true });
                    }
                    if (foundSubscribe.status == false)
                    {
                        return Json(new { dd = false });
                    }
                }
                
               
                if (foundSubscribe != null)
                {
                    cart.count = 1;
                    foundSubscribe.status = false;

                    db.SaveChanges();
                    // Tính giảm giá (30%)
                    decimal discountAmount = cart.Tong * 0.3m;

                    // Áp dụng giảm giá cho từng mục trong giỏ hàng
                    if (discountAmount > 0)
                    {
                        cart.TongAll = discountAmount;

                    }


                    // Cập nhật giỏ hàng trong Session
                    Session["Cart"] = cart;
                    return Json(new { Success = true });
                }

                else if (db.Subscribes.Any(x => x.MaCode != MaCode))
                {
                    return Json(new { rong = true });
                }





                
               
                else
                {
                    return Json(new { Success = false });
                }

            }
            return Json(new { Success = false });
        }






        [HttpPost]
        [AllowAnonymous]
       
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return Json(new { Success = true });
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                 
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", StaticRescouces.ResourceUser.taikhoanhoacmatkhaukhonghople);
                        return View(model);
                }
            }

            
           return  View("CheckOut", model);
        }
















        [AllowAnonymous]
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CountDem = cart.count;
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckOutSuccess()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult Partial_CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.Cart = cart;
                ViewBag.vv = cart.count;
                decimal tongValue = cart.Tong;
                if (cart.TongAll != 0)
                {
                    ViewBag.TongAll = cart.TongAll;

                }
                else
                {
                    ViewBag.TongAll = tongValue;
                }
                return PartialView(cart.Items);
            }
           
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult Partial_CheckThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            ViewBag.CountDem = cart.count;
            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                ViewBag.User = user;
                
                // Load XML file
                string xmlFilePath = Server.MapPath(@"~/Content/data/Provinces_Data.xml");
                XDocument doc = XDocument.Load(xmlFilePath);

                // Get the user's province ID
                string userProvinceId = user.ProvinceID.ToString(); // Assuming user has a property named ProvinceId

                // Extract province information from XML
                var province = doc.Descendants("Item")
                    .Where(item => item.Attribute("id")?.Value == userProvinceId && item.Attribute("type")?.Value == "province")
                    .FirstOrDefault();

                if (province != null)
                {
                    // Get the value of the province
                    string provinceValue = province.Attribute("value")?.Value;

                    // Pass the province value to the view
                    ViewBag.UserProvinceValue = provinceValue;

                    // Get the list of provinces for the dropdown
                    var provinces = doc.Descendants("Item")
                        .Where(item => item.Attribute("type")?.Value == "province")
                        .Select(item => new SelectListItem
                        {
                            Value = item.Attribute("id")?.Value,
                            Text = item.Attribute("value")?.Value
                        });

                    // Pass the SelectList to the view
                    ViewBag.ProvinceList = new SelectList(provinces, "Value", "Text", userProvinceId);

                    var district = doc.Descendants("Item")
      .Where(item => item.Attribute("type")?.Value == "district")
      .Select(item => new SelectListItem
      {
          Value = item.Attribute("id")?.Value,
          Text = item.Attribute("value")?.Value
      })
      .FirstOrDefault();

                    // Pass the SelectListItem to the view
                    ViewBag.DistrictList = new SelectList(new List<SelectListItem> { district }, "Value", "Text", userProvinceId);







                    var precinct = doc.Descendants("Item")
     .Where(item => item.Attribute("type")?.Value == "precinct")
     .Select(item => new SelectListItem
     {
         Value = item.Attribute("id")?.Value,
         Text = item.Attribute("value")?.Value
     })
     .FirstOrDefault();

                    // Pass the SelectListItem to the view
                    ViewBag.PrecinctList = new SelectList(new List<SelectListItem> { precinct }, "Value", "Text", userProvinceId);





                }
            }

            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return Json(new
                {

                    Count = cart.Items.Count,

                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {

                Count = 0
            }, JsonRequestBehavior.AllowGet);
        }



     



        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(X => X.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefaul) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefaul).image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    cart.count = 0;
                    cart.TongAll = 0;
                    if(cart.TongAll == 0)
                    {
                        cart.TongAll = cart.Tong;
                    }
                    Session["Cart"] = cart;
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
               
                cart.ClearCart();
                cart.count = 0;
                cart.TongAll = 0;
                Session["Cart"] = cart;
                return Json(new { Success = true });
            }
            cart.TongAll = 0;
            Session["Cart"] = cart;
            return Json(new { Success = false });
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Update(int id, int quatity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quatity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [AllowAnonymous]
        public JsonResult UpdateCartAll(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<ShoppingCartItem>>(cartModel);
            var sessionCart = (ShoppingCart)Session["Cart"];

            if (sessionCart != null)
            {
                foreach (var jsonItem in jsonCart)
                {
                    var cartItem = sessionCart.Items.SingleOrDefault(x => x.ProductId == jsonItem.ProductId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = jsonItem.Quantity;
                        cartItem.TotalPrice = cartItem.Price * cartItem.Quantity;
                    }
                }
            }

            Session["Cart"] = sessionCart; // Lưu lại giỏ hàng sau khi cập nhật
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CheckOut(OrderViewModel order)
        {
            var code = new { Success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null && cart.Items.Any())
                {
                    Order order1 = new Order();

                    order1.CustomerName = order.CustomerName;
                    order1.Phone = order.Phone;
                    order1.Address = order.Address;
                    order1.Email = order.Email;
                    order1.Status = 1;//chua thanh toan
                    order1.ProvinceID = order.ProviceID;
                    order1.DistrictID = order.DistrictID;
                    order1.WardID = order.WardID;
                    order1.notthink = order.notthink;
                    order1.Quantity = 0;
                    cart.Items.ForEach(x =>
                    {
                        order1.orderDetails.Add(new OrderDetail
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity,
                            Price = x.Price
                        });

                        // Sum up the quantities
                        order1.Quantity += x.Quantity;

                    });
                    order1.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order1.TypePayment = order.TypePayment;
                    order1.CreateDate = DateTime.Now;
                    order1.ModifiedDate = DateTime.Now;
                    order1.CreateBy = order.Phone;
                    if(User.Identity.IsAuthenticated)
                    {
                        order1.CustomerId = User.Identity.GetUserId();
                    }
                   
                    Random rd = new Random();
                    order1.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

                    db.Orders.Add(order1);

                    db.SaveChanges();
                    foreach (var orderDetail in order1.orderDetails)
                    {
                        var product = db.Products.Find(orderDetail.ProductId);
                        if (product != null)
                        {
                            // Update the product quantity
                            product.Quantity -= orderDetail.Quantity;
                            product.Warranty += orderDetail.Quantity;
                        }
                    }

                    db.SaveChanges();
                    //gửi email
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var tongtien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    tongtien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/layoutsendemail/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order1.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order1.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order1.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", order1.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiGiaoHang}}", order1.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", Common.Common.FormatNumber(tongtien, 0));
                    Common.Common.SendMail("Shop bán linh kiện máy tính DV Computer", "Đơn hàng #" + order1.Code, contentCustomer.ToString(), order1.Email);
                    //"Bạn click vào <a href='" + callbackUrl + "'>link này </a> rể reset mật khẩu"

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/layoutsendemail/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order1.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order1.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order1.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", order1.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiGiaoHang}}", order1.Address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", Common.Common.FormatNumber(tongtien, 0));
                    Common.Common.SendMail("Shop bán linh kiện máy tính DV Computer", "Đơn hàng mới #" + order1.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);

                    if(order.TypePayment == 1)
                    {
                        cart.ClearCart();
                        code = new { Success = true, Code = order.TypePayment, Url = "" };
                    }
                    if (order.TypePayment == 2)
                    {
                        cart.ClearCart();
                        var url = UrlPayment(order.TypePaymentVN, order1.Code);
                       
                        code = new { Success = true, Code = order.TypePayment, Url = url };
                    }
                    if (order.TypePayment == 3)
                    {
                        var result = PaymentWithPaypal(order1.Code);
                        if (result is RedirectResult redirectResult)
                        {
                            // Extract the URL from the RedirectResult
                            string paypalRedirectUrl = redirectResult.Url;

                            // Further processing or saving the URL as needed
                            // For example, you can store it in the 'code' variable
                            code = new { Success = true, Code = order.TypePayment, Url = paypalRedirectUrl };
                        }
                    }
             
                }
            }
            return Json(code);
        }




        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key
                                                                                        //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (Price).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreateDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàg:" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return urlPayment;
        }

        [AllowAnonymous]
        public ActionResult VnPayReturun()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }


                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if(itemOrder != null)
                        {
                            itemOrder.Status = 2;
                            db.Orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
               
            }
            return View();
        }




        /////////////////////////////////////////////
        [AllowAnonymous]
        public ActionResult PaymentWithPaypal(string orderCode)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, orderCode);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            cart.ClearCart();
            //on successful payment, show success page to user.  
            return View("CheckOutSuccess");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string orderCode)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

           
            //Adding Item Details like name, currency, price etc  
            cart.Items.ForEach(x =>
            {

                itemList.items.Add(new Item()
                {
                    name = x.ProductName,
                    currency = "USD",
                    price = Math.Round(x.Price / 24310, 0).ToString(),
                    quantity = x.Quantity.ToString(),
                    sku = "sku"
                });
            });
           
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = itemList.items.Sum(x => decimal.Parse(x.price) * int.Parse(x.quantity)).ToString(),
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (decimal.Parse(details.tax) + decimal.Parse(details.shipping) + decimal.Parse(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }


    }
}