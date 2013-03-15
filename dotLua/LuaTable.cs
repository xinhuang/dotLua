using System;
using System.Dynamic;

namespace dotLua
{
    public class LuaTable : DynamicObject, IDisposable
    {
        private readonly ILuaState _luaState;
        private readonly string _key;

        public LuaTable(ILuaState luaState, int index)
        {
            _luaState = luaState;
            CheckType(index);
            _key = _luaState.SetRegistry(index);
        }

        private void CheckType(int index)
        {
            LuaType type = _luaState.Type(index);
            if (type != LuaType.Table)
                throw new TypeMismatchException(string.Format("Expecting table but got {0}.", type));
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            int top = _luaState.GetTop();
            try
            {
                _luaState.Push(_key);
                _luaState.GetTable((int)LuaIndex.Registry);
                _luaState.Push(binder.Name);
                _luaState.GetTable(-2);
                result = _luaState.StackAt(-1);
            }
            finally
            {
                _luaState.SetTop(top);
            }

            return true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            _luaState.ClearRegistry(_key);
        }
    }
}