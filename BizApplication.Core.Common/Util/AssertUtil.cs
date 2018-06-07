using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.Util
{
    public static class AssertUtil
    {
        public static bool IsNull(object value)
        {
            return ReferenceEquals(value, null);
        }

        public static bool IsNotNull(object value)
        {
            return !IsNull(value);
        }

        public static void AssertNotNull(object value)
        {
            if (IsNull(value))
            {
                throw new ArgumentNullException("Nullが入力されました");
            }
        }
    }
}
