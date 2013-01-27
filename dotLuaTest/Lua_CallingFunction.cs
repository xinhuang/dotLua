﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class Lua_CallingFunction
    {
        private readonly Mock<ILuaState> _mockLuaState = new Mock<ILuaState>();
        private dynamic _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new Lua(_mockLuaState.Object);
        }

        [TestMethod]
        public void given_calling_lua_script_function_without_param_should_call_to_LuaState()
        {
            _sut.Foo();

            _mockLuaState.Verify(o => o.Call("Foo",
                                             It.Is<object[]>(args => args.Length == 0)),
                                 Times.Exactly(1));
        }

        [TestMethod]
        public void given_calling_lua_script_function_with_1_param_should_call_to_LuaState()
        {
            _sut.Foo(1);

            _mockLuaState.Verify(o => o.Call("Foo",
                                             It.Is<object[]>(args => args.Length == 1 && (int) args[0] == 1)
                                          ), Times.Exactly(1));
        }

        [TestMethod, ExpectedException(typeof(InvocationException))]
        public void given_lua_pcall_return_non_zero_should_throw_invocation_exception()
        {
            _mockLuaState.Setup(o => o.Call("Error", It.IsAny<object[]>()))
                .Returns(LuaError.Runtime);

            _sut.Error();
        }
    }
}
