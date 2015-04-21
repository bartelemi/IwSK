using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Describes type of message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Ping request
        /// </summary>
        PingRequest,

        /// <summary>
        /// Ping response
        /// </summary>
        PingResponse,

        /// <summary>
        /// Ordinary message
        /// </summary>
        Plain,

        /// <summary>
        /// Transaction - send and wait for response
        /// </summary>
        Transaction,

        /// <summary>
        /// None or message processed
        /// </summary>
        None,
    }
}
