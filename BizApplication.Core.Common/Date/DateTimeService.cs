using BizApplication.Core.Common.CoreIF;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.Date
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset GetLocalNow()
        {
            return DateTimeOffset.Now;
        }

        public DateTimeOffset GetUtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
