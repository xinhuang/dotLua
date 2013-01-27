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
            }
        }
    }
}
