using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class LuaIntegrationTest
    {
        private dynamic _sut;

        [TestInitialize]
        public void SetUp()
        {
            _sut = new Lua();
            _sut.Do("test.lua");
        }

        [TestCleanup]
        public void TearDown()
        {
            _sut.Dispose();
        }

        [TestMethod]
        public void calling_lua_function_without_arguments()
        {
            _sut.say_hi();
        }
    } 
}
