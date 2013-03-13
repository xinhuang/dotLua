namespace dotLua
{
    public class InvocationException : DotLuaException
    {
        public InvocationException(LuaError error, string function)
            : base(string.Format("{0} error when calling function \"{1}\"", error, function))
        {
        }

        public InvocationException()
        {
        }
    }
}