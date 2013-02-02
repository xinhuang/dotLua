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

                try
                {
                    lua.raise("Boom!");
                    Console.WriteLine("Error: Lua error should be converted to an exception!");
                }
                catch (InvocationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
