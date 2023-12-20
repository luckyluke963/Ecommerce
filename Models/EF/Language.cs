using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EF
{
    [Table("tb_Language")]
    public class Language
    {
        [StringLength(2)]
        public string ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}
