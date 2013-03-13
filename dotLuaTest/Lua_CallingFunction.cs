using System.Collections.Generic;
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
            _mockLuaState.Setup(o => o.PCall(0, LuaState.MultiReturn, 0)).Returns(LuaError.Error);

            _sut.Error();
        }

        [TestMethod]
        public void given_calling_lua_function_return_a_string_should_correct_result_received()
        {
            _mockLuaState.Setup(o => o.TypeOf(FunctionName)).Returns(LuaType.Function);
            int top = 0;
            _mockLuaState.Setup(o => o.GetTop()).Returns(() => top);
            _mockLuaState.Setup(o => o.PCall(0, LuaState.MultiReturn, 0)).Returns(LuaError.Ok).Callback(() => top = 1);
            _mockLuaState.Setup(o => o.StackAt(1)).Returns("expect");

            IList<dynamic> actual = _sut.Foo();

            _mockLuaState.Verify(o => o.GetGlobal(FunctionName), Times.Exactly(1));
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("expect", actual[0]);
        }
    }
}