using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    public class ThongKeTruyCap
    {
        public static string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public static ThongKeViewModel ThongKe()
        {
            using(var connect = new SqlConnection(connection))
            {
                var item = connect.QueryFirstOrDefault<ThongKeViewModel>("thongke", CommandType.StoredProcedure);
                return item;
            }
        }
    }
}
