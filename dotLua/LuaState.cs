using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using lua_Number = System.Double;

namespace dotLua
{
    internal class LuaState : ILuaState
    {
        private readonly IntPtr _luaState = luaL_newstate();
        private int _registryIndex = (int) LuaRegisterIndex.Last;

        public static Type NumberType
        {
            get { return typeof (lua_Number); }
        }

        public void OpenLibs()
        {
            luaL_openlibs(_luaState);
        }

        public int GetTop()
        {
            return lua_gettop(_luaState);
        }

        public void GetGlobal(string name)
        {
            lua_getglobal(_luaState, name);
        }

        public dynamic StackAt(int index)
        {
            LuaType type = lua_type(_luaState, index);
            return type.GetValue(this, index);
        }

        public Tuple<LuaType, object> GetField(string name)
        {
            lua_getglobal(_luaState, name);
            LuaType type = lua_type(_luaState, -1);
            var result = new Tuple<LuaType, object>(type, type.GetValue(this, -1));
            lua_pop(_luaState, 1);
            return result;
        }

        public LuaType TypeOf(string name)
        {
            lua_getglobal(_luaState, name);
            LuaType type = lua_type(_luaState, -1);
            lua_pop(_luaState, 1);
            return type;
        }

        public List<dynamic> GetStackRange(int index, int n)
        {
            if (n == 0)
                return null;
            var results = new List<dynamic>(n);
            Enumerable.Range(index + 1, n).ForEach(i => results.Add(StackAt(i)));
            return results;
        }

        public string SetRegistry(int index)
        {
            string registryIndex = NewRegistryIndex();
            Push(registryIndex);
            PushNil();
            Copy(-3, index);
            SetTable((int)LuaIndex.Registry);
            return registryIndex;
        }

        public void ClearRegistry(string index)
        {
            Push(index);
            PushNil();
            SetTable((int)LuaIndex.Registry);
        }

        public LuaType Type(int index)
        {
            return lua_type(_luaState, index);
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

        public LuaError Do(string filename)
        {
            return luaL_dofile(_luaState, filename);
        }

        public void Push(lua_Number value)
        {
            lua_pushnumber(_luaState, value);
        }

        public void PushNil()
        {
            lua_pushnil(_luaState);
        }

        public void Copy(int source, int dest)
        {
            lua_copy(_luaState, source, dest);
        }

        public void GetTable(int index)
        {
            lua_gettable(_luaState, index);
        }

        public void Pop()
        {
            lua_pop(_luaState, 1);
        }

        public string NewRegistryIndex()
        {
            checked
            {
                return "REG_" + (++_registryIndex).ToString();
            }
        }

        public void SetTable(int index)
        {
            lua_settable(_luaState, index);
        }

        public void Push(string value)
        {
            lua_pushstring(_luaState, value);
        }

        public LuaError PCall(int nArgs, int multiReturn, int errFunc)
        {
            return lua_pcall(_luaState, nArgs, multiReturn, errFunc);
        }

        public void SetTop(int index)
        {
            lua_settop(_luaState, index);
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
        private static extern LuaError lua_pcallk(IntPtr luaState, int nArgs, int nRet, int errFunc, int context,
                                                  LuaCFunction hook);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaError lua_getglobal(IntPtr luaState, string name);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern LuaError lua_gettable(IntPtr luaState, int index);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lua_gettop(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int lua_settop(IntPtr luaState, int index);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_settable(IntPtr luaState, int index);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_pushnumber(IntPtr luaState, lua_Number value);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_pushnil(IntPtr luaState);

        [DllImport("Lua.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void lua_copy(IntPtr luaState, int source, int dest);

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
            return lua_pcall(luaState, 0, LuaConstant.MultiReturn, 0);
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

        private void lua_pop(IntPtr luaState, int n)
        {
            lua_settop(luaState, -n - 1);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
#if DEBUG
            int top = lua_gettop(_luaState);
            if(top != 0)
                throw new UnbalanceStackException(-top, StackAt(top).ToString());
#endif
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                lua_close(_luaState);
            }
            GC.SuppressFinalize(this);
        }

        ~LuaState()
        {
            Dispose(false);
        }

        #endregion

        public LuaError Load(string filename)
        {
            return luaL_loadfile(_luaState, filename);
        }

        private delegate int LuaCFunction(IntPtr luaState);
    }
}