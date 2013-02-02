using System;

namespace dotLua
{
    public interface ILuaState : IDisposable
    {
        void OpenLibs();
        LuaError Do(string filename);

        LuaError Call(string functionName, dynamic[] args);
        Tuple<LuaType, object> GetField(string name);
        LuaType TypeOf(string name);

        bool ToBoolean(int index);
        double ToNumber(int index);
        string ToString(int index);
    }
}
