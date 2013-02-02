using System;

namespace dotLua
{
    public class DotLuaException : Exception
    {
        protected DotLuaException(string message)
            : base(message)
        {
        }

        protected DotLuaException()
        {
        }
    }
}