using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EF
{
    [Table("tb_ProductCategory")]
    public class ProductCategory :CommonAbstract
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        //[Required]
        [StringLength(250)]
        public string Alias { get; set; }
        public string Description { get; set; }

       

        public long? ParentID { get; set; }

        public int? DisplayOrder { get; set; }
        [StringLength(250)]
        public string Icon { get; set; }
        [StringLength(250)]
        public string SeoTile { get; set; }
        [StringLength(500)]
        public string SeoDescription { get; set; }
        [StringLength(250)]
        public string SeoKeywords { get; set; }

        public bool Status { get; set; }

        public bool? ShowOnHome { get; set; }

        [StringLength(2)]
        public string Language { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
