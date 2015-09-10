using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Exceptions
{
    class InvalidChecksumException : Exception
    {
        public InvalidChecksumException(string message)
            : base(message)
        {

        }
    }
}
