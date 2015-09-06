using System;

namespace RS485.Common.Exceptions
{
    public class InternalErrorException : Exception
    {
        public InternalErrorException(String message)
            : base(message)
        {
        }
    }
}
