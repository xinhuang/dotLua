namespace dotLua
{
    enum LuaRegisterIndex
    {
        MainThread = 1,
        Globals = 2,
        Last = Globals,
    }

    enum LuaIndex
    {
        MaxStack = 1000000,
        FirstPseudoIndex = - MaxStack - 1000,
        Registry = FirstPseudoIndex,
    }
}