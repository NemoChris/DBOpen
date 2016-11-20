using System;
using DBOpen.Controller;

using NUnit.Framework;

namespace DBOpen.Test
{

    [TestFixture]
    public class MyTest
    {
        [Test]
        public void GetDefaultValueByTypeName()
        {
            // Gets the default values for different types
            Type t = Type.GetType("System.Int32", true);            
            object value = t.IsValueType ? Activator.CreateInstance(t) : null; // 0

            Type t7 = typeof(byte);
            object value7 = t7.IsValueType ? Activator.CreateInstance(t7) : null; // 0

            Type t4 = typeof(short);
            object value4 = t4.IsValueType ? Activator.CreateInstance(t4) : null; // 0

            Type t3 = typeof(long);
            object value3 = t3.IsValueType ? Activator.CreateInstance(t3) : null; // 0
            
            Type t2 = typeof(Boolean);
            object value2 = t2.IsValueType ? Activator.CreateInstance(t2) : null; // false
            
            Type t5 = typeof(float);
            object value5 = t5.IsValueType ? Activator.CreateInstance(t5) : null; // 0.0

            Type t6 = typeof(double);
            object value6 = t6.IsValueType ? Activator.CreateInstance(t6) : null; // 0.0


        }
    }
}
