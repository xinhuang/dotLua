using System;

namespace dotLua
{
    public interface ILuaState : IDisposable
    {
        void OpenLibs();
        LuaError Do(string filename);

        LuaError Call(string functionName, dynamic[] args);
    }
}