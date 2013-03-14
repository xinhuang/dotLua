namespace dotLua
{
    public class TypeMismatchException : DotLuaException
    {
        public TypeMismatchException(string message)
            : base(message)
        {
        }
    }
}