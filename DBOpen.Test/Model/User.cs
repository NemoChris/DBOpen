using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOpen.Test.Model
{
    public class User
    {
        public static string Conn = "mybatis";
        public static string TableName = "`user`";
        public static string IDFieldName = "id";

        public int id { get; set; }
        public string username { get; set; }
        public DateTime birthday { get; set; }

        public string sex { get; set; }
        public string address { get; set; }

    }
}
