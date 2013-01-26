using System;
using System.Runtime.InteropServices;
using System.Text;

namespace dotLua
{
    public class LuaState : IDisposable
    {
        private delegate int LuaCFunction(IntPtr luaState);

        private readonly IntPtr _luaState = luaL_newstate();
        private const int MultiReturn = -1;

        public int Load(string filename)
        {
            return luaL_loadfile(_luaState, filename);
        }

        public int Do(string filename)
        {
            return luaL_dofile(_luaState, filename);
        }

        #region Imported Lua Functions

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr luaL_newstate();

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_close(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void luaL_openlibs(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int luaL_loadfilex(IntPtr luaState, string filename, string mode);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lua_pcallk(IntPtr luaState, int nArgs, int nRet, int errFunc, int context, LuaCFunction hook);

        private static int luaL_loadfile(IntPtr luaState, string filename)
        {
            return luaL_loadfilex(luaState, filename, null);
        }

        private static int luaL_dofile(IntPtr luaState, string filename)
        {
            int err = luaL_loadfile(luaState, filename);
            if (err != 0)
                return err;
            return lua_pcall(luaState, 0, LuaState.MultiReturn, 0);
        }

        private static int lua_pcall(IntPtr luaState, int nArgs, int nRet, int errFunc)
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
