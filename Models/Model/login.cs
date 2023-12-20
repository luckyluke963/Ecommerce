using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class login
    {
        public int id { get; set; } 
        public string FullName { get; set; }
       
     
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "tài khoản")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }
    }
}
