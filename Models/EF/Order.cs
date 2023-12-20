using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EF
{
    [Table("tb_Order")]
    public class Order : CommonAbstract
    {
        public Order()
        {
            orderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage ="Tên khách hàng không được để trống")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage ="Số điện thoại không để trống")]
        public string Phone { get; set; }
        [Required(ErrorMessage ="Địa chỉ không để trống")]
        public string Address { get; set; }
        public string Email { get; set; }

        public int? ProvinceID { get; set; }

        public int? DistrictID { get; set; }

        public int? WardID { get; set; }





        public decimal TotalAmount { get; set; }




        public string notthink { get;set; }

        public int Quantity { get; set; }

        public int TypePayment { get; set; }
        public string CustomerId { get; set; }  
        public int Status { get; set; }
        public virtual ICollection<OrderDetail> orderDetails { get; set; }
    }
}
