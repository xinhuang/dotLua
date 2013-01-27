using System;
using System.Dynamic;

namespace dotLua
{
    public class Lua : DynamicObject, IDisposable
    {
        private readonly ILuaState _luaState;

        public Lua() 
            : this(new LuaState())
        {
        }

        internal Lua(ILuaState luaState)
        {
            _luaState = luaState;
            _luaState.OpenLibs();
        }

        public void Do(string filename)
        {
            _luaState.Do(filename);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = new LuaObject(this, binder);
            return true;
        }

        public dynamic Invoke(string functionName, dynamic[] args)
        {
            if (_luaState.Call(functionName, args) != LuaError.Ok)
                throw new InvocationException("Error when invoking " + functionName);
            return null;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _luaState.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        ~Lua()
        {
            Dispose(false);
        }

        #endregion
    }
}