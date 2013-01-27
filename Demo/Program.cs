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

                try
                {
                    lua.raise("Boom!");
                }
                catch (InvocationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
