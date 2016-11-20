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
    public class MySqlControllerTest
    {
        // Test Insert
        [Test]
        public void InsertTest()
        {
            Console.WriteLine("Test Insert");
            IController ic = ControllerFactory.CreateController();

            User u = new User();

            u.username = "Doraemon";
            u.address = "Mars";
            u.birthday = DateTime.Now;
            u.sex = "1";

            bool isSuc = ic.Insert(u);

        }

        // Test Update
        [Test]
        public void UpdateTest()
        {
            Console.WriteLine("Test Update");
            IController ic = ControllerFactory.CreateController();

            User u = new User();
            u.id = 44;
            ic.Fill(u);
            u.address = "earth";

            bool isSuc = ic.Update(u);

        }

        // Test Fill
        [Test]
        public void FillTest2()
        {
            Console.WriteLine("Test Fill");
            IController ic = ControllerFactory.CreateController();

            User u = new User();
            u.id = 44;


            bool isSuc = ic.Fill(u);

        }

        // Test Delete
        [Test]
        public void DelTest()
        {
            Console.WriteLine("Test DelTest");
            IController ic = ControllerFactory.CreateController();

            User u = new User();
            u.id = 44;


            bool isSuc = ic.Delete(u);

        }


        // Test  DelBatch
        [Test]
        public void DelBatchTest()
        {
            Console.WriteLine("Test DelBatchTest");
            IController ic = ControllerFactory.CreateController();

            bool isSuc = ic.Delete<User>("42,43,45");

        }

        // Test Save1
        [Test]
        public void SaveTest1()
        {
            Console.WriteLine("Test SaveTest1");
            IController ic = ControllerFactory.CreateController();


            User u = new User();

            u.username = "Doraemon";
            u.address = "Mars";
            u.birthday = DateTime.Now;
            u.sex = "1";



            bool isSuc = ic.Save(u);

        }

        // Test Save2
        [Test]
        public void SaveTest2()
        {
            Console.WriteLine("Test SaveTest2");
            IController ic = ControllerFactory.CreateController();


            User u = new User();

            u.username = "Doraemon";
            u.address = "earth";
            u.birthday = DateTime.Now;
            u.sex = "1";
            u.id = 45;

            bool isSuc = ic.Save(u);

        }

    }
}
