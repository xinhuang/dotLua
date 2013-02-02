﻿using System;
using System.Dynamic;
using System.IO;

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
            LuaError error = _luaState.Do(filename);
            if (error != LuaError.Ok)
                throw new FileLoadException(error.ToString());
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Tuple<LuaType, object> field = _luaState.GetField(binder.Name);

            switch (field.Item1)
            {
                case LuaType.String:
                case LuaType.Number:
                result = field.Item2;
                    break;

                default:
                    throw new NotImplementedException(string.Format("Lua object {0} of type {1} is not supported.",
                                                                    binder.Name, field.Item1));
            }
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_luaState.TypeOf(binder.Name) != LuaType.Function)
            {
                result = null;
                return false;
            }

            result = Invoke(binder.Name, args);
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