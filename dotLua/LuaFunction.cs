using System;
using System.Dynamic;

namespace dotLua
{
    public class LuaFunction : DynamicObject
    {
        private readonly Lua _lua;
        private readonly GetMemberBinder _binder;
        private readonly LuaType _type;

        public LuaFunction(Lua lua, GetMemberBinder binder, LuaType type)
        {
            _lua = lua;
            _binder = binder;
            _type = type;
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
