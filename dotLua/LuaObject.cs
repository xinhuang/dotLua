using System;
using System.Dynamic;

namespace dotLua
{
    public class LuaObject : DynamicObject
    {
        private readonly Lua _lua;
        private readonly GetMemberBinder _binder;
        private readonly LuaType _type;

        public LuaObject(Lua lua, GetMemberBinder binder, LuaType type)
        {
            _lua = lua;
            _binder = binder;
            _type = type;
        }

        public static implicit operator double(LuaObject value)
        {
            return value._lua.GetDouble(value._binder.Name);
        }

        public static implicit operator int(LuaObject value)
        {
            return value._lua.GetInt(value._binder.Name);
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            if (_type != LuaType.Function)
                throw new Exception("Invocation can only made on function.");
            result = _lua.Invoke(_binder.Name, args);
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = _lua.TryGet(_binder.Name, binder.ReturnType);
            return true;
        }
    }
}
