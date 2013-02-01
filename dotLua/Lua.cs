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
            if (_luaState.TypeOf(binder.Name) == LuaType.Number)
                result = _luaState.GetField(binder.Name).Item2;
            else
                result = new LuaFunction(this, binder, _luaState.TypeOf(binder.Name));
            return true;
        }

        public dynamic Invoke(string functionName, dynamic[] args)
        {
            LuaError error = _luaState.Call(functionName, args);
            if (error != LuaError.Ok)
                throw new InvocationException(error, functionName);
            return null;
        }

        public object TryGet(string name, Type type)
        {
            return _luaState.GetField(name).Item1;
        }
        
        public double GetDouble(string name)
        {
            return (double)_luaState.GetField(name).Item2;
        }

        public int GetInt(string name)
        {
            return (int)_luaState.GetField(name).Item2;
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