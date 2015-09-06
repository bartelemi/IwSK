using System;

namespace RS485.Master.Serial.Exceptions
{
    class InvalidArgumentException: Exception
    {
        public InvalidArgumentException(string message)
            : base(message)
        {

        }
    }
}
