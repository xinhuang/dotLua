using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace dotLua
{
    public class LuaTable : DynamicObject, IDisposable
    {
        private readonly ILuaState _luaState;
        private readonly string _key;

        public LuaTable(ILuaState luaState, int index)
        {
            _luaState = luaState;
            CheckType(index);
            _key = _luaState.SetRegistry(index);
        }

        private void CheckType(int index)
        {
            LuaType type = _luaState.Type(index);
            if (type != LuaType.Table)
                throw new TypeMismatchException(string.Format("Expecting table but got {0}.", type));
        }

        public dynamic this[string index]
        {
            get { return GetValue(index); }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetValue(binder.Name);

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            int top = _luaState.GetTop();
            try
            {
                _luaState.Push(_key);
                _luaState.GetTable((int)LuaIndex.Registry);
                _luaState.Push(binder.Name);
                _luaState.GetTable(-2);
                result = Call(args);
            }
            finally
            {
                _luaState.SetTop(top);
            }
            return true;
        }

        private IList<dynamic> Call(dynamic[] args)
        {
            int bottom = _luaState.GetTop();

            try
            {
                RawCall(args);
                return GetResults(bottom);
            }
            finally
            {
                _luaState.SetTop(bottom);
            }
        }

        private IList<dynamic> GetResults(int bottom)
        {
            int top = _luaState.GetTop();

            int nArgs = top - bottom;
            List<dynamic> results = null;
            if (nArgs > 0)
            {
                results = GetStackRange(bottom, nArgs);
            }
            return results;
        }

        private void RawCall(dynamic[] args)
        {
            args.ForEach(arg => _luaState.Push(arg));
            LuaError error = _luaState.PCall(args.Length, LuaConstant.MultiReturn, 0);
            if (error != LuaError.Ok)
            {
                throw new InvocationException(error, "Table:MemberFunction");
            }
        }

        private List<dynamic> GetStackRange(int index, int n)
        {
            var results = new List<dynamic>(n);
            Enumerable.Range(index + 1, n).ForEach(i => results.Add(_luaState.StackAt(i)));
            return results;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = GetValue(indexes[0].ToString());
            return true;
        }

        private dynamic GetValue(string index)
        {
            int top = _luaState.GetTop();
            try
            {
                _luaState.Push(_key);
                _luaState.GetTable((int) LuaIndex.Registry);
                _luaState.Push(index);
                _luaState.GetTable(-2);
                return _luaState.StackAt(-1);
            }
            finally
            {
                _luaState.SetTop(top);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            _luaState.ClearRegistry(_key);
        }

        ~LuaTable()
        {
            Dispose(false);
        }
    }
}