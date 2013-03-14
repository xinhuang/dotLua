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
            _luaState.PushNil();
            _luaState.Copy(-2, -1);
            _luaState.Push(_registryIndex);
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
            _luaState.Push(_registryIndex);
            _luaState.GetTable((int)LuaIndex.Registry);
            result = _luaState.StackAt(-1);
            _luaState.Pop();
            return true;
        }
    }
}