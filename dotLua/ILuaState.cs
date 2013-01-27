﻿using System;

namespace dotLua
{
    public interface ILuaState : IDisposable
    {
        void OpenLibs();
        int Do(string filename);

        int Call(string functionName);
        int Call(string functionName, dynamic[] arg0);
    }
}