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
            SetupField("IntField", LuaType.Number, expect);

            Assert.AreEqual(expect, _sut.IntField);
        }

        [TestMethod]
        public void given_query_global_string_field_should_return_currect_value()
        {
            const string expect = "don't care";

            SetupField("StringField", LuaType.String, expect);

            Assert.AreEqual(expect, _sut.StringField);
        }

        [TestMethod]
        public void given_query_global_boolean_field_should_return_currect_value()
        {
            const bool expect = true;

            SetupField("BooleanField", LuaType.Boolean, expect);

            Assert.AreEqual(expect, _sut.BooleanField);
        }

        private void SetupField<T>(string fieldName, LuaType expectType, T expect)
        {
            _mockLuaState.Setup(o => o.GetField(fieldName))
                         .Returns(new Tuple<LuaType, object>(expectType, expect));
            _mockLuaState.Setup(o => o.TypeOf(fieldName)).Returns(expectType);
        }
    }
}