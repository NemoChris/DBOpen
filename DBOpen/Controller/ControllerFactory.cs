using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace DBOpen.Controller
{
    public class ControllerFactory
    {
        static string ControllerName = ConfigurationManager.AppSettings["DBOpenControllerName"];
        public static IController CreateController()
        {
            // Reflection create controller object,Case sensitive
            return System.Reflection.Assembly.GetExecutingAssembly().CreateInstance("DBOpen.Controller." + ControllerName + "Controller", false) as IController;
        }
    }
}
