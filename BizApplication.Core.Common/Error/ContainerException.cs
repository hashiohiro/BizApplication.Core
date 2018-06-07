using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BizApplication.Core.Common.Error
{
    public class ContainerException : CoreException
    {
        public ContainerException()
        {
        }

        public ContainerException(string message) : base(message)
        {
        }

        public ContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContainerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
