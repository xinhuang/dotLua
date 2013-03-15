using System;
using System.Collections.Generic;
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

        [TestMethod]
        public void calling_lua_function_with_1_argument()
        {
            _sut.say_hi("dotLua");
        }

        [TestMethod]
        public void calling_lua_function_with_3_argument()
        {
            _sut.say_hi(Math.PI, "dotLua", "Sunday");
        }

        [TestMethod]
        public void given_query_global_double_var_in_script_should_return_correct_value()
        {
            Assert.AreEqual(3, _sut.GlobalNumber);
        }

        [TestMethod]
        public void given_query_global_string_var_in_script_should_return_correct_value()
        {
            Assert.AreEqual("Good luck~", _sut.GlobalString);
        }

        [TestMethod]
        public void given_query_global_boolean_var_in_script_should_return_correct_value()
        {
            Assert.IsTrue(_sut.GlobalBoolean);
        }

        [TestMethod]
        public void given_calling_lua_function_returning_1_double_should_result_be_received()
        {
            IList<dynamic> result0 = _sut.return_0();

            Assert.AreEqual(1, result0.Count);
            Assert.AreEqual(0, result0[0]);
        }

        [TestMethod]
        public void given_calling_lua_function_returning_2_double_should_result_be_received()
        {
            IList<dynamic> result01 = _sut.return_0_1();

            Assert.AreEqual(2, result01.Count);
            Assert.AreEqual(0, result01[0]);
            Assert.AreEqual(1, result01[1]);
        }

        [TestMethod]
        public void given_calling_lua_function_returning_double_and_string_should_result_be_received()
        {
            IList<dynamic> result0str = _sut.return_0_str();

            Assert.AreEqual(2, result0str.Count);
            Assert.AreEqual(0, result0str[0]);
            Assert.AreEqual("Hi~", result0str[1]);
        }

        [TestMethod, ExpectedException(typeof (InvocationException))]
        public void given_lua_script_execute_error_should_throw_invocation_exception()
        {
            _sut.raise("Boom!");
        }

        [TestMethod]
        public void given_query_a_table_should_a_table_object_returns()
        {
            Assert.IsNotNull(_sut.GlobalTable);
        }

        [TestMethod]
        public void given_query_a_table_field_via_dot_name_should_correct_value_returns()
        {
            Assert.AreEqual(3.14, _sut.GlobalTable.Field);
        }
    }
}