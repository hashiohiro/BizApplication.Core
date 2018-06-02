using BizApplication.Core.Common.CoreContainer;
using BizApplication.Core.Common.CoreIF;
using BizApplication.Core.Common.Date;
using BizApplication.Core.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BizApplication.Core.Test.Common
{
    public class Test_CoreContainer
    {
        private ICoreContainer container = new CoreContainer(new CoreInstanceCache(new DateTimeService()), new DynamicCodeHelper());

        private interface ITestClass
        {
            int TestId { get; }
        }
        private class TestClass_Transient : ITestClass
        {
            public int TestId { get; set; }
        }

        private class TestClass_Singleton : ITestClass
        {
            public int TestId { get; set; }
        }

        [Fact]
        public void Test_Register_Transient()
        {
            container.Register(typeof(ITestClass), typeof(TestClass_Transient), ObjectLifeTimes.Transient, 0);
            container.Build();
            container.Resolve(typeof(ITestClass));
        }

        [Fact]
        public void Test_Register_Singleton()
        {
            container.Register(typeof(ITestClass), typeof(TestClass_Singleton), ObjectLifeTimes.Singleton, 0);
            container.Build();
            container.Resolve(typeof(ITestClass));
        }
    }
}
