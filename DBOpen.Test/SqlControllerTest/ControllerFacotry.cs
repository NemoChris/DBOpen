using System;
using DBOpen.Controller;

using NUnit.Framework;


namespace DBOpen.Test.SqlControllerTest
{
    [TestFixture]
    public class ControllerFacotry
    {
        [Test]
        public void CreateController()
        {
            Console.WriteLine("ControllerFactory Start creating objects");
            IController ic = ControllerFactory.CreateController();

            if (ic != null)
            {
                Console.WriteLine("Create object success！");
            }
            else
            {
                Assert.Fail("Failed to create an object, the return value is null!");
            }

            bool isSqlCon = ic is SqlController;

            Console.WriteLine("Whether to SqlController objects：：" + isSqlCon);

        }
    }
}
