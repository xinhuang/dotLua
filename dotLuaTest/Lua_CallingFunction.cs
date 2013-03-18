using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class Lua_CallingFunction : LuaTestBase
    {
        private const string FunctionName = "Foo";

        [TestMethod]
        public void given_calling_lua_script_function_without_param_should_call_to_LuaState()
        {
            _mockLuaState.Setup(o => o.TypeOf(FunctionName)).Returns(LuaType.Function);

            _sut.Foo();

            _mockLuaState.Verify(o => o.Push(It.Is<string>(param => param != FunctionName)), Times.Never());
        }

        [TestMethod]
        public void given_calling_lua_script_function_with_1_param_should_call_to_LuaState()
        {
            _mockLuaState.Setup(o => o.TypeOf(FunctionName)).Returns(LuaType.Function);

            _sut.Foo(1);

            _mockLuaState.Verify(o => o.Push(1), Times.Exactly(1));
        }

        [TestMethod, ExpectedException(typeof (InvocationException))]
        public void given_lua_pcall_return_non_zero_should_throw_invocation_exception()
        {
            _mockLuaState.Setup(o => o.TypeOf("Error")).Returns(LuaType.Function);
            _mockLuaState.Setup(o => o.PCall(0, LuaConstant.MultiReturn, 0)).Returns(LuaError.Error);

            _sut.Error();
        }
    }
}