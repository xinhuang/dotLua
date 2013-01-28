using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using dotLua;

namespace dotLuaTest
{
    public class LuaTestBase
    {
        protected readonly Mock<ILuaState> _mockLuaState = new Mock<ILuaState>();
        protected dynamic _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new Lua(_mockLuaState.Object);
        }
    }
}