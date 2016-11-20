using System;
using System.Data;

using DBOpen.Controller;
using DBOpen.Test.Model;

using NUnit.Framework;
using MySql.Data.MySqlClient;

using sys = System.Collections.Generic;


namespace DBOpen.Test.MySqlControllerTest
{
    /// <summary>
    /// BaseController Test class
    /// </summary>
    [TestFixture]
    public class BaseControllerTest
    {
        // Test QueryDataSet
        [Test]
        public void QueryDataSetWithParTest()
        {
            Console.WriteLine("Test QueryDataSet");
            IController ic = ControllerFactory.CreateController();
            MySqlParameter[] pars = new MySqlParameter[] { new MySqlParameter("test", "1") };
            DataSet ds = ic.Query("mybatis", "SELECT * FROM `user`", pars);
        }

        // Test execution of the stored procedure returns DataSet
        [Test]
        public void QueryDataSetSPTest()
        {
            Console.WriteLine("Test execution of the stored procedure returns DataSet");
            IController ic = ControllerFactory.CreateController();
            // The parameter name must be consistent with the parameter name defined in the stored procedure 
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("_id", MySqlDbType.Int32) { Value = 37 } };

            DataSet dt = ic.Query("mybatis", "$proc2",parameters);
        }

        // Test QueryList
        [Test]
        public void QueryListTest()
        {
            Console.WriteLine("Test QueryList");
            IController ic = ControllerFactory.CreateController();
            MySqlParameter[] pars = new MySqlParameter[] { new MySqlParameter("@id", SqlDbType.Int) { Value = 37 } };
            sys.List<User> list = ic.Query<User>("SELECT * FROM `user` WHERE id=@id ;", pars);

        }
        
        [Test]
        public void NonQueryTest()
        {
            Console.WriteLine("NonQueryTest");
            IController ic = ControllerFactory.CreateController();
            string sql = @"
               insert into `user`(username,birthday,sex,address)
               values(@username,@birthday,@sex,@address)";

            int count = 2;
            MySqlParameter[] pars = new MySqlParameter[] 
            { 
                new MySqlParameter("@username", MySqlDbType.VarChar) { Value = "hpy"+count} ,
                new MySqlParameter("@birthday", MySqlDbType.DateTime) { Value = DateTime.Now } ,
                new MySqlParameter("@sex", MySqlDbType.VarChar) { Value = "1" } ,
                new MySqlParameter("@address", MySqlDbType.VarChar) { Value = "Beijing" } 

            };

            int affectCount = ic.NonQuery("mybatis", sql, pars);
        }
    }
}
