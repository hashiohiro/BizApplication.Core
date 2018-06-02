using BizApplication.Core.Common.CoreContainer;
using BizApplication.Core.Common.CoreIF;
using BizApplication.Core.Common.Date;
using BizApplication.Core.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Dynamic Code Helper
            IDynamicCodeHelper helper = new DynamicCodeHelper();
            var agg = new ILPropertyAggregateTest();
            var test = helper.GetProperties(agg);
            helper.SetPropertyValue(agg, nameof(ILPropertyAggregateTest.Id), 99);

            // Core Container
            var container = new CoreContainer(new CoreInstanceCache(new DateTimeService()), new DynamicCodeHelper());
            container.RegisterByCustomAttribute(Assembly.Load("ConsoleApp"), typeof(RegisterTypeAttribute));
            container.Build();
            var obj = container.Resolve(typeof(ILPropertyAggregateTest));
        }
    }

    [RegisterType(typeof(ILPropertyAggregateTest), ObjectLifeTimes.Transient, 0)]
    public class ILPropertyAggregateTest
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public IList<string> Refs { get; set; }
        public IDictionary<string, object> ExtendedProperties { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public decimal TotalAmount { get; set; }
        public bool DeleteFlag { get; set; }

        public IList<PropertyInfo> PropertyList()
        {
            return GetType().GetProperties();
        }
    }

    [RegisterType(typeof(ILPropertyAggregateTest), ObjectLifeTimes.Transient, 1)]
    public class ILPropertyAggregateTest2 : ILPropertyAggregateTest
    {
        public string Test2 { get; set; }
    }
}
