namespace dotLua
{
    internal class UnbalanceStackException : DotLuaException
    {
        public UnbalanceStackException(int count, string topValue)
            : base(string.Format("Unbalanced Lua stack with {0} items left. Top is <{1}>",
                count, topValue))
        {
        }
    }
}