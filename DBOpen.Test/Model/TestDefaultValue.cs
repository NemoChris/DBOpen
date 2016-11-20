using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOpen.Test.Model
{
    public class TestDefaultValue
    {

        public static string Conn = "book_shop3";
        public static string TableName = "[Test_DefaultValue]";
        public static string IDFieldName = "ID";
        public int ID { get; set; }
        public int p1 { get; set; }
        public bool p2 { get; set; }
        public string p3 { get; set; }
        public byte p4 { get; set; }
        public DateTime p5 { get; set; }
        public double p6 { get; set; }
        public decimal p7 { get; set; }
    }
}
