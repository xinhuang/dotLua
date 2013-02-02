using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class Lua_CallingFunction : LuaTestBase
    {
        [TestMethod]
        public void given_calling_lua_script_function_without_param_should_call_to_LuaState()
        {
            _mockLuaState.Setup(o => o.TypeOf("Foo")).Returns(LuaType.Function);

            _sut.Foo();

            _mockLuaState.Verify(o => o.Call("Foo",
                                             It.Is<object[]>(args => args.Length == 0)),
                                 Times.Exactly(1));
        }

        [TestMethod]
        public void given_calling_lua_script_function_with_1_param_should_call_to_LuaState()
        {
            _mockLuaState.Setup(o => o.TypeOf("Foo")).Returns(LuaType.Function);

            _sut.Foo(1);

            _mockLuaState.Verify(o => o.Call("Foo",
                                             It.Is<object[]>(args => args.Length == 1 && (int) args[0] == 1)
                                          ), Times.Exactly(1));
        }

        [TestMethod, ExpectedException(typeof(InvocationException))]
        public void given_lua_pcall_return_non_zero_should_throw_invocation_exception()
        {
            _mockLuaState.Setup(o => o.TypeOf("Error")).Returns(LuaType.Function);
            _mockLuaState.Setup(o => o.Call("Error", It.IsAny<object[]>()))
                .Throws(new InvocationException());

            _sut.Error();
        }

        [TestMethod]
        public void given_calling_lua_function_return_a_string_should_correct_result_received()
        {
            _mockLuaState.Setup(o => o.TypeOf("Foo")).Returns(LuaType.Function);
            _mockLuaState.Setup(o => o.Call("Foo", It.IsAny<object[]>()))
                         .Returns(new List<dynamic> { "expect" });

            IList<dynamic> actual = _sut.Foo();

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("expect", actual[0]);
        }
    }
}
