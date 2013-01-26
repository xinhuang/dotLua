﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotLua;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var luaState = new LuaState())
            {
                if (luaState.Do("test.lua") != 0)
                {
                    Console.WriteLine("Load file test.lua failed.");
                }
            }
        }
    }
}