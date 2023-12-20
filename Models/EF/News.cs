using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.EF
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "tieudekhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.Resources))]
        [StringLength(150)]
        public string Title { get; set; }

        public string Alias { get; set; }


        [Required(ErrorMessageResourceName = "baivietkhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.Resources))]
        [AllowHtml]
      
        public string Detail { get; set; }

        [Required(ErrorMessageResourceName = "hinhanhkhongduocrong", ErrorMessageResourceType = typeof(StaticRescouces.Resources))]
        public string Image { get; set; }

      

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public string SeoKeywords { get; set; }

        public bool IsActive { get; set; }

        public virtual Category Category { get; set; }
    }
}
