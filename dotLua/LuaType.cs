using System;

namespace dotLua
{
    public enum LuaType
    {
        None = -1,
        Nil = 0,
        Boolean = 1,
        LightUserData = 2,
        Number = 3,
        String = 4,
        Table = 5,
        Function = 6,
        UserData = 7,
        Thread = 8,
    }

    public static class LuaTypeMethods
    {
        public static dynamic GetValue(this LuaType self, ILuaState luaState, int index)
        {
            switch (self)
            {
            case LuaType.Boolean:
                return luaState.ToBoolean(index);

            case LuaType.Number:
                return luaState.ToNumber(index);

            case LuaType.String:
                return luaState.ToString(index);

            case LuaType.Table:
                return new LuaTable(luaState, index);

            case LuaType.Nil:
                return null;

            default:
                throw new NotImplementedException(string.Format("Try get {0} of type {1}", index, self));
            }
        }
    }
}