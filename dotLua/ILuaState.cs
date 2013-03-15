using System;

namespace dotLua
{
    public interface ILuaState : IDisposable
    {
        void OpenLibs();

        LuaError Do(string filename);

        LuaType TypeOf(string name);

        bool ToBoolean(int index);
        double ToNumber(int index);
        string ToString(int index);

        void GetGlobal(string name);
        Tuple<LuaType, object> GetField(string name);
        LuaError PCall(int nArgs, int multiReturn, int errFunc);
        dynamic StackAt(int i);

        void SetTop(int index);
        int GetTop();

        void Push(string value);
        void Push(double value);
        void GetTable(int index);
        void Pop();

        int NewRegistryIndex();
        void SetTable(int index);
        LuaType Type(int index);
        void PushNil();
        void Copy(int source, int dest);

        int SetRegistry(int index);
    }
}