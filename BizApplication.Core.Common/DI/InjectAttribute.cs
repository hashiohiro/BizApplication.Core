using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.DI
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
    }
}
