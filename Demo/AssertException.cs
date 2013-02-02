using System;

namespace Demo
{
    internal class AssertException : Exception
    {
        public AssertException(string message)
            : base(message)
        {
        }
    }
}