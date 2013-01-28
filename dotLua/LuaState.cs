using System;
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

        public LuaError Call(string functionName, dynamic[] args)
        {
            lua_getglobal(_luaState, functionName);
            args.ForEach(arg => Push(arg));
            return lua_pcall(_luaState, args.Length, 0, 0);
        }

        public Tuple<LuaType, object> GetField(string name)
        {
            throw new NotImplementedException();
        }

        public LuaType TypeOf(string name)
        {
            lua_getglobal(_luaState, name);
            return lua_type(_luaState, 0);
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
        private static extern void lua_pushnumber(IntPtr luaState, lua_Number value);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_pushstring(IntPtr luaState, string value);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaType lua_type(IntPtr luaState, int index);

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
