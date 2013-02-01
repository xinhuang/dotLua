using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class Lua_AccessVariable : LuaTestBase
    {
        [TestMethod]
        public void given_access_query_int_field_for_int_should_return_currect_value()
        {
            const int expect = 42;
            _mockLuaState.Setup(o => o.GetField("IntField"))
                         .Returns(new Tuple<LuaType, object>(LuaType.Number, 42));
            _mockLuaState.Setup(o => o.TypeOf("IntField")).Returns(LuaType.Number);

            Assert.AreEqual(expect, _sut.IntField);
        }
    }
}