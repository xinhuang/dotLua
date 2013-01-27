using System;

namespace dotLua
{
    public interface ILuaState : IDisposable
    {
        int Call(string functionName);
        int Do(string filename);
        void OpenLibs();
    }
}