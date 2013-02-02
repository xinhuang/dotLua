using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using lua_Number = System.Double;

namespace dotLua
{
    class LuaState : ILuaState
    {
        private delegate int LuaCFunction(IntPtr luaState);

        private readonly IntPtr _luaState = luaL_newstate();

        private const int MultiReturn = -1;

        public static Type NumberType
        {
            get { return typeof (lua_Number); }
        }

        public void OpenLibs()
        {
            luaL_openlibs(_luaState);
        }

        public IList<dynamic> Call(string functionName, dynamic[] args)
        {
            int before = lua_gettop(_luaState);

            lua_getglobal(_luaState, functionName);
            args.ForEach(arg => Push(arg));
            LuaError error = lua_pcall(_luaState, args.Length, MultiReturn, 0);
            if (error != LuaError.Ok)
                throw new InvocationException(error, functionName);

            int after = lua_gettop(_luaState);

            int nArgs = after - before;
            var results = new List<dynamic>(nArgs);
            Enumerable.Range(0, nArgs).ForEach(i => results.Add(StackAt(i)));

            lua_settop(_luaState, before);
            return results;
        }

        private dynamic StackAt(int index)
        {
            LuaType type = lua_type(_luaState, index);
            return type.GetValue(this, index);
        }

        public Tuple<LuaType, object> GetField(string name)
        {
            var type = TypeOf(name);
            lua_getglobal(_luaState, name);
            return new Tuple<LuaType, object>(type, type.GetValue(this, -1));
        }

        public LuaType TypeOf(string name)
        {
            lua_getglobal(_luaState, name);
            return lua_type(_luaState, -1);
        }

        public bool ToBoolean(int index)
        {
            return lua_toboolean(_luaState, index) != 0;
        }

        public double ToNumber(int index)
        {
            return lua_tonumber(_luaState, index);
        }

        public string ToString(int index)
        {
            return lua_tostring(_luaState, index);
        }

        public LuaError Load(string filename)
        {
            return luaL_loadfile(_luaState, filename);
        }

        public LuaError Do(string filename)
        {
            return luaL_dofile(_luaState, filename);
        }

        private void Push(double value)
        {
            lua_pushnumber(_luaState, value);
        }

        private void Push(string value)
        {
            lua_pushstring(_luaState, value);
        }

        #region Imported Lua Functions

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr luaL_newstate();

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_close(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void luaL_openlibs(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaError luaL_loadfilex(IntPtr luaState, string filename, string mode);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaError lua_pcallk(IntPtr luaState, int nArgs, int nRet, int errFunc, int context, LuaCFunction hook);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaError lua_getglobal(IntPtr luaState, string name);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lua_gettop(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lua_settop(IntPtr luaState, int index);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_pushnumber(IntPtr luaState, lua_Number value);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_pushstring(IntPtr luaState, string value);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaType lua_type(IntPtr luaState, int index);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern lua_Number lua_tonumberx(IntPtr luaState, int index, out bool isNum);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr lua_tolstring(IntPtr luaState, int index, out int length);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lua_toboolean(IntPtr luaState, int index);

        private static LuaError luaL_loadfile(IntPtr luaState, string filename)
        {
            return luaL_loadfilex(luaState, filename, null);
        }

        private static LuaError luaL_dofile(IntPtr luaState, string filename)
        {
            LuaError err = luaL_loadfile(luaState, filename);
            if (err != 0)
                return err;
            return lua_pcall(luaState, 0, LuaState.MultiReturn, 0);
        }

        private static LuaError lua_pcall(IntPtr luaState, int nArgs, int nRet, int errFunc)
        {
            return lua_pcallk(luaState, nArgs, nRet, errFunc, 0, null);
        }

        private static lua_Number lua_tonumber(IntPtr luaState, int index)
        {
            bool isNumber;
            return lua_tonumberx(luaState, index, out isNumber);
        }

        private string lua_tostring(IntPtr luaState, int index)
        {
            int length;
            return Marshal.PtrToStringAnsi(lua_tolstring(luaState, index, out length));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            lua_close(_luaState);
            GC.SuppressFinalize(this);
        }

        ~LuaState()
        {
            Dispose(false);
        }

        #endregion
    }
}
