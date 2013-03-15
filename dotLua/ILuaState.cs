using System;

namespace dotLua
{
    public interface ILuaState : IDisposable
    {
        void OpenLibs();

        LuaError Do(string filename);

        bool ToBoolean(int index);
        double ToNumber(int index);
        string ToString(int index);

        void GetGlobal(string name);
        LuaError PCall(int nArgs, int multiReturn, int errFunc);

        void SetTop(int index);
        int GetTop();

        void PushNil();
        void Push(string value);
        void Push(double value);
        void Pop();
        void Copy(int source, int dest);

        void GetTable(int index);
        void SetTable(int index);

        LuaType Type(int index);

        LuaType TypeOf(string name);
        Tuple<LuaType, object> GetField(string name);
        dynamic StackAt(int i);

        string NewRegistryIndex();
        string SetRegistry(int index);
        void ClearRegistry(string index);
    }
}