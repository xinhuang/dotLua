using System;
using dotLua;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var luaState = new LuaState())
            {
                luaState.OpenLibs();
                if (luaState.Do("test.lua") != 0)
                {
                    Console.WriteLine("Load file test.lua failed.");
                }
                if (luaState.Call("say_hi") != 0)
                {
                    Console.WriteLine("Calling function say_hi failed.");
                }
            }
        }
    }
}
