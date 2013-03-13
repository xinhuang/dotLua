using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace dotLua
{
    public class Lua : DynamicObject, IDisposable
    {
        private readonly ILuaState _luaState;

        public Lua()
            : this(new LuaState())
        {
        }

        internal Lua(ILuaState luaState)
        {
            _luaState = luaState;
            _luaState.OpenLibs();
        }

        public void Do(string filename)
        {
            LuaError error = _luaState.Do(filename);
            if (error != LuaError.Ok)
                throw new FileLoadException(error.ToString());
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Tuple<LuaType, object> field = _luaState.GetField(binder.Name);

            switch (field.Item1)
            {
            case LuaType.String:
            case LuaType.Number:
            case LuaType.Boolean:
                result = field.Item2;
                break;

            default:
                throw new NotImplementedException(string.Format("Lua object {0} of type {1} is not supported.",
                                                                binder.Name, field.Item1));
            }
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_luaState.TypeOf(binder.Name) != LuaType.Function)
            {
                result = null;
                return false;
            }

            result = Call(binder.Name, args);
            return true;
        }

        private IList<dynamic> Call(string functionName, dynamic[] args)
        {
            int bottom = _luaState.GetTop();

            try
            {
                RawCall(functionName, args);
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

        private void RawCall(string functionName, dynamic[] args)
        {
            _luaState.GetGlobal(functionName);
            args.ForEach(arg => _luaState.Push(arg));
            LuaError error = _luaState.PCall(args.Length, LuaState.MultiReturn, 0);
            if (error != LuaError.Ok)
            {
                throw new InvocationException(error, functionName);
            }
        }

        private List<dynamic> GetStackRange(int index, int n)
        {
            var results = new List<dynamic>(n);
            Enumerable.Range(index + 1, n).ForEach(i => results.Add(_luaState.StackAt(i)));
            return results;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _luaState.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        ~Lua()
        {
            Dispose(false);
        }

        #endregion
    }
}