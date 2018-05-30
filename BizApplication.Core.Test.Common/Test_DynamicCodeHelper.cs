using BizApplication.Core.Common.CoreIF;
using BizApplication.Core.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace BizApplication.Core.Test.Common
{
    public class Test_DynamicCodeHelper
    {
        private IDynamicCodeHelper helper = new DynamicCodeHelper();

        private class TestClass
        {
            public TestClass() { }
            public TestClass(bool boolValue)
            {
                Bool = boolValue;
            }

            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public sbyte SByte { get; set; }
            public char Char { get; set; }
            public double Double { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public uint UInt { get; set; }
            public long Long { get; set; }
            public ulong ULong { get; set; }
            public short Short { get; set; }
            public ushort UShort { get; set; }
            public object Object { get; set; }
        }


        [Theory]
        [InlineData(true)]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        [InlineData(SByte.MaxValue)]
        [InlineData(SByte.MinValue)]
        [InlineData('a')]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData("Text")]
        public void Test_GetType(object value)
        {
            Assert.Equal(value.GetType(), helper.GetType(value));
        }

        [Fact]
        public void Test_GetType_Null()
        {

            Assert.Equal(null, helper.GetType(null));
        }

        [Fact]
        public void Test_GetProperties()
        {
            var obj = new TestClass();
            var exp = new List<string>()
            {
                "Bool",
                "Byte",
                "SByte",
                "Char",
                "Double",
                "Float",
                "Int",
                "UInt",
                "Long",
                "ULong",
                "Short",
                "UShort",
                "Object",
            };
            Assert.Equal(exp, helper.GetProperties(obj).Select(x => x.Name));
        }

        [Fact]
        public void Test_GetProperties_Null()
        {
            Assert.Equal(null, helper.GetProperties(null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        [InlineData(SByte.MaxValue)]
        [InlineData(SByte.MinValue)]
        [InlineData('a')]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData("Text")]
        public void Test_PropertyValue(object value)
        {
            var obj = new TestClass();
            helper.SetPropertyValue(obj, "Object", value);

            Assert.Equal(value, helper.GetPropertyValue(obj, "Object"));
        }

        [Fact]
        public void Test_NullPropertyValue()
        {
            Assert.ThrowsAny<ArgumentNullException>(() =>
            {
                helper.GetPropertyValue(null, "Object");
            });

            var obj = new TestClass();
            Assert.ThrowsAny<ArgumentNullException>(() =>
            {
                helper.GetPropertyValue(obj, null);
            });
        }

        [Fact]
        public void Test_CreateInstance()
        {
            var exp = new TestClass();
            var expType = exp.GetType();

            var act = helper.CreateInstance(expType);
            var actType = act.GetType();
            Assert.Equal(expType.AssemblyQualifiedName, actType.AssemblyQualifiedName);
        }

        [Fact]
        public void Test_CreateInstanceWithNull()
        {
            Assert.ThrowsAny<ArgumentNullException>(() =>
            {
                helper.CreateInstance(null);
            });
        }

        [Fact]
        public void Test_CreateInstanceWithArgs()
        {
            var exp = new TestClass(true);
            var expType = exp.GetType();

            var act = helper.CreateInstance(expType, true);
            var actType = act.GetType();
            Assert.Equal(expType.AssemblyQualifiedName, actType.AssemblyQualifiedName);
        }

        [Fact]
        public void Test_CreateInstanceWithNullArgs()
        {
            var obj = new TestClass(true);
            Assert.ThrowsAny<MissingMethodException>(() =>
            {
                helper.CreateInstance(obj.GetType(), 1);
            });
        }
    }
}
