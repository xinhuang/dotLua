using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotLua;

namespace dotLuaTest
{
    [TestClass]
    public class Lua_AccessVariable : LuaTestBase
    {
        [TestMethod]
        public void given_query_global_double_field_for_int_should_return_currect_value()
        {
            const int expect = 42;
            _mockLuaState.Setup(o => o.GetField("IntField"))
                         .Returns(new Tuple<LuaType, object>(LuaType.Number, 42));
            _mockLuaState.Setup(o => o.TypeOf("IntField")).Returns(LuaType.Number);

            Assert.AreEqual(expect, _sut.IntField);
        }

        [TestMethod]
        public void given_query_global_string_field_should_return_currect_value()
        {
            const string expect = "don't care";
            const string fieldName = "StringField";

            _mockLuaState.Setup(o => o.GetField(fieldName))
                         .Returns(new Tuple<LuaType, object>(LuaType.String, expect));
            _mockLuaState.Setup(o => o.TypeOf(fieldName)).Returns(LuaType.String);

            Assert.AreEqual(expect, _sut.StringField);
        }
    }
}