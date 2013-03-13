namespace dotLua
{
    internal class UnbalanceStackException : DotLuaException
    {
        public UnbalanceStackException(int count) 
            : base(string.Format("Unbalanced Lua stack with {0} items left.", count))
        {
        }
    }
}