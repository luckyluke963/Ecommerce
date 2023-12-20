using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EF
{
    [Table("tb_Category")]
    public class Category : CommonAbstract
    {
        public Category()
        {
            News = new HashSet<News>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
     
        [Required(ErrorMessageResourceName = "tendanhmuckhongdcrong", ErrorMessageResourceType = typeof(StaticRescouces.Resources))]
        [StringLength(150)]
        [Display(Name ="Category_Name", ResourceType = typeof(StaticRescouces.Resources))]
        public string Title { get; set; } //name

        public string Alias { get; set; } //metaTitle

        public long? ParentID { get; set; }

        public int? DisplayOrder { get; set; }


        public string Description { get; set; }
        [StringLength(150)]
        public string SeoTitle { get; set; }
        [StringLength(250)]
        public string SeoDescription { get; set; }
        [StringLength(150)]
        public string SeoKeywords { get; set; }

       
        [Required(ErrorMessageResourceName = "thutukhongdcrong", ErrorMessageResourceType = typeof(StaticRescouces.Resources))]
        public int Position { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }

        public bool? Status { get; set; }

        public bool? ShowOnHome { get; set; }
        public bool IsActive { get; set; }
        [StringLength(2)]
        public string Language { get; set; }

        public ICollection<News> News { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}
