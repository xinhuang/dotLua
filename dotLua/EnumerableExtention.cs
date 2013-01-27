using System;
using System.Collections.Generic;

namespace dotLua
{
    static class EnumerableExtention
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (T value in self)
            {
                action(value);
            }
        }
    }
}