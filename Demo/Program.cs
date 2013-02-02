using System;
using System.Collections.Generic;
using dotLua;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (dynamic lua = new Lua())
            {
                lua.Do("test.lua");
                lua.say_hi();
                lua.say_hi("dotLua");
                lua.say_hi(3.1415926);
                lua.say_hi(3.1415926, "dotLua", "Sunday");

                Console.WriteLine("Global Value: {0}", lua.GlobalNumber);
                Assert.AreEqual(3, lua.GlobalNumber);
                Console.WriteLine("Global Value: {0}", lua.GlobalString);
                Assert.AreEqual("Good luck~", lua.GlobalString);
                Assert.IsTrue(lua.GlobalBoolean);

                IList<dynamic> result0 = lua.return_0();
                Assert.AreEqual(1, result0.Count);
                Assert.AreEqual(0, result0[0]);

                IList<dynamic> result01 = lua.return_0_1();
                Assert.AreEqual(2, result01.Count);
                Assert.AreEqual(0, result01[0]);
                Assert.AreEqual(1, result01[1]);

                IList<dynamic> result0str = lua.return_0_str();
                Assert.AreEqual(2, result0str.Count);
                Assert.AreEqual(0, result0str[0]);
                Assert.AreEqual("Hi~", result0str[1]);

                try
                {
                    lua.raise("Boom!");
                    throw new Exception();
                }
                catch (InvocationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
