using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Exceptions
{
    class InternalErrorException : Exception
    {
        public InternalErrorException(String message)
            : base(message)
        {

        }
    }
}
