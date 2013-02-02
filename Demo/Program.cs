using System;
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
