using System;
using System.Runtime.InteropServices;

namespace dotLua
{
    public class LuaState : IDisposable
    {
        private readonly IntPtr _luaState = luaL_newstate();

        [DllImport("Lua.dll")]
        private static extern IntPtr luaL_newstate();

        [DllImport("Lua.dll")]
        private static extern void lua_close(IntPtr luaState);

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            lua_close(_luaState);
        }

        ~LuaState()
        {
            Dispose(false);
        }
    }
}
