using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
