using System;
using System.Data;
using System.Data.SqlClient;

using DBOpen.Controller;
using DBOpen.Test.Model;

using NUnit.Framework;

using  sys=System.Collections.Generic;

namespace DBOpen.Test.SqlControllerTest
{
    [TestFixture]
    public class BaseControllerTest
    {        
        [Test]
        public void QueryDataSetWithParTest()
        {
            Console.WriteLine("Start execution QueryDataSetWithParTest");
            IController ic = ControllerFactory.CreateController();
            SqlParameter[] pars = new SqlParameter[] { new SqlParameter("test", "1") };
            DataSet ds = ic.Query("book_shop3", "SELECT * FROM [Users];", pars);
        }

        [Test]
        public void QueryDataSetSPTest()
        {
            Console.WriteLine("Start execution QueryDataSetSPTest");
            IController ic = ControllerFactory.CreateController();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@start", SqlDbType.Int){Value=0} ,
                new SqlParameter("@end", 10) ,
                new SqlParameter("@category", 1) ,
                new SqlParameter("@order", "id") 
            };
            DataSet dt = ic.Query("book_shop3", "$Pro_GetPagedList", pars);
        }

        [Test]
        public void QueryListTest()
        {
            Console.WriteLine("Start execution QueryListTest");
            IController ic = ControllerFactory.CreateController();
            SqlParameter[] pars = new SqlParameter[] { new SqlParameter("@Sort", SqlDbType.Int){Value=2} };
            sys.List<ActionGroup> list = ic.Query<ActionGroup>("SELECT * FROM [ActionGroup] WHERE [Sort]=@Sort ;", pars);
        }
        [Test]
        public void NonQueryTest()
        {
            Console.WriteLine("Start execution NonQueryTest");
            IController ic = ControllerFactory.CreateController();
            string sql = @"
                insert into 
	                [book_shop3].[dbo].[ActionGroup]([GroupName],[GroupType],[DelFlag],[Sort])
                values(@GroupName,@GroupType,@DelFlag,@Sort)";

            int count = 2;
            SqlParameter[] pars = new SqlParameter[] 
            { 
                new SqlParameter("@GroupName", SqlDbType.NVarChar) { Value = "a"+count} ,
                new SqlParameter("@GroupType", SqlDbType.Int) { Value = 2 } ,
                new SqlParameter("@DelFlag", SqlDbType.NVarChar) { Value = "Normal" } ,
                new SqlParameter("@Sort", SqlDbType.Int) { Value = 3 } 

            };

            int affectCount = ic.NonQuery("book_shop3", sql, pars);
        }

        [Test]
        public void QueryListTestDefaultValue()
        {
            Console.WriteLine("Start execution QueryListTestDefaultValue");
            IController ic = ControllerFactory.CreateController();

            sys.List<TestDefaultValue> list = ic.Query<TestDefaultValue>("SELECT * FROM [Test_DefaultValue]");
        }
    }
}
