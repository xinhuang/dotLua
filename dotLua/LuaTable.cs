using System.Dynamic;

namespace dotLua
{
    public class LuaTable : DynamicObject
    {
        private readonly ILuaState _luaState;
        private readonly int _registryIndex;

        public LuaTable(ILuaState luaState, int index)
        {
            _luaState = luaState;
            CheckType(index);
            _registryIndex = _luaState.SetRegistry(index);
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
                _luaState.Push(_registryIndex);
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
    }
}