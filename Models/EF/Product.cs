using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbstract
    {
        public Product() { 
            this.ProductImage = new HashSet<ProductImage>();
            this.OrderDetail = new HashSet<OrderDetail>();
            this.Reviews = new HashSet<ReviewProduct>();
            this.Wishlists = new HashSet<Wishlist>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public string Alias { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        [Column(TypeName = "xml")]
        public string MoreImage { get; set; }
        public decimal Price { get; set; }
        public decimal? PromotionPrice { get; set; }

        public bool? IncludeVAT { get; set; }
        public decimal? PriceSale { get; set; }

        public int? Warranty { get; set; }

        public int Quantity { get; set; }

        public bool IsHome { get; set; }
        public bool IsSale { get; set; }

        public decimal OriginalPrice { get; set; }

        public bool IsFeature { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        public bool IsHot { get; set; }

        public int ProductCategoryId { get; set; }

        [StringLength(250)]
        public string SeoTitle { get; set; }
        [StringLength(250)]
        public string SeoDescription { get; set; }
        [StringLength(250)]
        public string SeoKeywords { get; set; }

        public DateTime? TopHot { get; set; }

        public int ViewCount { get; set; }

        public bool IsActive { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }

        public virtual ICollection<ReviewProduct> Reviews { get; set; }

        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
