using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BizApplication.Core.Common.Error
{
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string message) : base(message)
        {
        }

        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
