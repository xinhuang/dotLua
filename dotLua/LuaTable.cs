using System;
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
            _registryIndex = _luaState.NewRegistryIndex();
            CheckType(index);
            StoreInRegistry(index);
        }

        private void StoreInRegistry(int index)
        {
            if (index != -1)
                throw new NotImplementedException();
            _luaState.Push(_registryIndex);
            _luaState.PushNil();
            _luaState.Copy(-3, -1);
            _luaState.SetTable((int)LuaIndex.Registry);
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
                var t = _luaState.Type(-1);
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