using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.CoreContainer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterTypeAttribute : Attribute
    {
        public RegisterTypeAttribute(
            Type abstractType,
            ObjectLifeTimes objectLifeTimes = ObjectLifeTimes.Transient,
            int priority = 0)
        {
            AbstractType = abstractType;
            ObjectLifeTime = objectLifeTimes;
            Priority = priority;
        }

        public Type AbstractType { get; set; }
        public ObjectLifeTimes ObjectLifeTime { get; set; }
        public int Priority { get; set; }
    }
}
