using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class RecentProduct
    {
        public int ProductId { get; set; }

        public string ProdutName { get; set; }

        public string image { get; set; }
        public decimal price { get; set; }  

        public decimal? priceSale { get; set; }

        public string alas { get; set; }


        public DateTime LastVisited { get; set; }

    }
}
