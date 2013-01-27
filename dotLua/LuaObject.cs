using System;
using System.Dynamic;

namespace dotLua
{
    public class LuaObject : DynamicObject
    {
        private readonly Lua _lua;
        private readonly GetMemberBinder _binder;

        public LuaObject(Lua lua, GetMemberBinder binder)
        {
            _lua = lua;
            _binder = binder;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            if (args.Length > 0)
                throw new NotImplementedException("Lua function with arguments is not supported.");

            result = _lua.Invoke(_binder.Name);
            return true;
        }
    }
}