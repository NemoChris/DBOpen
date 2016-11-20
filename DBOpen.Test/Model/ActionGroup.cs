using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOpen.Test.Model
{
    public class ActionGroup
    {
        /// <summary>
        /// Database connection string 
        /// </summary>
        public static string Conn = "book_shop3";

        /// <summary>
        /// Database table name
        /// </summary>
        public static string TableName = "[ActionGroup]";

        /// <summary>
        /// primary key name
        /// </summary>
        public static string IDFieldName = "ID";

        public int ID { get; set; }
        public string GroupName { get; set; }
        public Int16 GroupType { get; set; }
        public string DelFlag { get; set; }
        public int Sort { get; set; }

    }
}
