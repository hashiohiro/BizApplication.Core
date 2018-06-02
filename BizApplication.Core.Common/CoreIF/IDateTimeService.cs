using System;
using System.Collections.Generic;
using System.Text;

namespace BizApplication.Core.Common.CoreIF
{
    
    /// <summary>
    /// Provide a way to get time.
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/4331189/datetime-vs-datetimeoffset
    /// </remarks>
    public interface IDateTimeService
    {
        /// <summary>
        /// Get the current local time
        /// </summary>
        /// <returns>Local DateTime</returns>
        DateTimeOffset GetLocalNow();

        /// <summary>
        /// Get the current UTC time
        /// </summary>
        /// <returns>Local DateTime</returns>
        DateTimeOffset GetUtcNow();
    }
}
