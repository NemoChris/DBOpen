using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOpen.Util
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Gets the default value for the type
        /// </summary>
        /// <param name="t">type</param>
        /// <returns>default value</returns>
        public static object GetDefaultValueByType(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
