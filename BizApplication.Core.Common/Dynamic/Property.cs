using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.Dynamic
{
    /// <summary>
    /// Provides processing necessary for dynamic access of properties
    /// </summary>
    public class Property
    {
        public Property(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public object GetValue()
        {
            throw new NotImplementedException();
        }

        public void SetValue(object value)
        {
            throw new NotImplementedException();
        }
    }
}
