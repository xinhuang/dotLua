using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class Lua_CallingFunction
    {
        private Mock<ILuaState> _mockLuaState = new Mock<ILuaState>();
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

            _mockLuaState.Verify(o => o.Call("Foo"), Times.Exactly(1));
        }
    }
}
