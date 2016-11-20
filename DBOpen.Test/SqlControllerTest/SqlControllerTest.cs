using System;
using System.Data;
using System.Data.SqlClient;

using DBOpen.Controller;
using DBOpen.Test.Model;

using NUnit.Framework;

using sys = System.Collections.Generic;

namespace DBOpen.Test.SqlControllerTest
{
    [TestFixture]
    public class SqlControllerTest
    {        
        [Test]
        public void InsertTest()
        {
            Console.WriteLine("Test Insert");
            IController ic = ControllerFactory.CreateController();

            ActionGroup ag = new ActionGroup();
            ag.GroupName = "a3";
            ag.GroupType = 2;
            ag.Sort = 2;
            ag.DelFlag = "Normal";

            bool isSuc = ic.Insert(ag);

        }

        [Test]
        public void UpdateTest()
        {
            Console.WriteLine("Test Update");
            IController ic = ControllerFactory.CreateController();

            ActionGroup ag = new ActionGroup();
            ag.GroupName = "a3";
            ag.GroupType = 2;
            ag.Sort = 2;
            ag.DelFlag = "Normal";
            ag.ID = 1;

            bool isSuc = ic.Update(ag);

        }

        // Test Fill
        [Test]
        public void FillTest()
        {
            Console.WriteLine("Test FillTest");
            IController ic = ControllerFactory.CreateController();

            ActionGroup ag = new ActionGroup();
            ag.ID = 1;


            bool isSuc = ic.Fill(ag);

        }

        // Test Delete
        [Test]
        public void DelTest()
        {
            Console.WriteLine("Test DelTest");
            IController ic = ControllerFactory.CreateController();

            ActionGroup ag = new ActionGroup();
            ag.ID = 12;


            bool isSuc = ic.Delete(ag);

        }

        // Test DelBatch
        [Test]
        public void DelBatchTest()
        {
            Console.WriteLine("Test DelBatchTest");
            IController ic = ControllerFactory.CreateController();

            bool isSuc = ic.Delete<ActionGroup>("10,11,12");

        }

        // Test Save1
        [Test]
        public void SaveTest1()
        {
            Console.WriteLine("Test SaveTest1");
            IController ic = ControllerFactory.CreateController();

            ActionGroup ag = new ActionGroup();
            ag.GroupName = "a4";
            ag.GroupType = 2;
            ag.Sort = 2;
            ag.DelFlag = "Normal";


            bool isSuc = ic.Save(ag);

        }


        // Test Save2
        [Test]
        public void SaveTest2()
        {
            Console.WriteLine("Test SaveTest2");
            IController ic = ControllerFactory.CreateController();

            ActionGroup ag = new ActionGroup();
            ag.GroupName = "a6";
            ag.GroupType = 2;
            ag.Sort = 2;
            ag.DelFlag = "Normal";
            ag.ID = 13;

            bool isSuc = ic.Save(ag);

        }

    }
}
