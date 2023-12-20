using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShopCart
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không để trống")]
        public string Address { get; set; }
        public string Email { get; set; }

        public int? ProviceID { get; set; }


        public int? DistrictID { get; set; }

        public int? WardID { get; set; }

        public int TypePayment { get; set; }
        public string notthink { get; set; }    

        public string CustomerId { get; set; }
        public int TypePaymentVN { get; set; }
    }
}
