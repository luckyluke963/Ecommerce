using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class RecentNews
    {
        public int NewsId { get; set; }

        public string NewsName { get; set; }

        public string image { get; set; }
       

        public string alas { get; set; }


        public DateTime LastVisited { get; set; }
    }
}
