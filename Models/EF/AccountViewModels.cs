using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Models.EF
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "taikhoankhongduocrong",ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
       
        [Display(Name = "UserName")]

        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "matkhaukhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class CreateAccountViewModel
    {
        [Required]
        public string UserName { get; set; }

      
        public DateTime CreatetAcount { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }


        public string Address { get; set; }

       
        public int? ProviceID { get; set; }

       
        public int? DistrictID { get; set; }

        public int? WardID { get; set; }

        public string Phone { get; set; }

        public List<string> Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }




    public class EditAccountViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }
        public string Phone { get; set; }

        public List<string> Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

       
    }


































    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "taikhoankhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        public string UserName { get; set; }

        public string CaptchaCode { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessageResourceName = "diachikhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        public string Address { get; set; }

        [Display(Name = "Tỉnh/thành")]
        public string ProviceID { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string DistrictID { get; set; }

        [Display(Name = "Phường/Xã")]
        public string WardID { get; set; }

      
        [Required(ErrorMessageResourceName = "hovatenkhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceName = "emailkhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        [EmailAddress(ErrorMessageResourceName = "Phaidungdinhdangemail", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "matkhaukhongduocrong",ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        [StringLength(100, ErrorMessageResourceName = "matkhauphaiitnhat",ErrorMessageResourceType =typeof(StaticRescouces.ResourceUser), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessageResourceName = "nhaplaimatkhaukhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        [Compare("Password", ErrorMessageResourceName = "matkhauvamatkhaukhongkhop",ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "emailkhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
        [EmailAddress(ErrorMessageResourceName = "Phaidungdinhdangemail", ErrorMessageResourceType = typeof(StaticRescouces.ResourceUser))]
       
        [Display(Name = "Email")]
        public string Email { get; set; }
    }






}
