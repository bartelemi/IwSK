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
        /// None or message processed
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Ordinary message
        /// </summary>
        Plain = 0x01,
        
        /// <summary>
        /// Ping request
        /// </summary>
        PingRequest = 0x02,

        /// <summary>
        /// Ping response
        /// </summary>
        PingResponse = 0x12,

        /// <summary>
        /// TransactionBegin - send and wait for response
        /// </summary>
        TransactionBegin = 0x03,

        /// <summary>
        /// TransactionEnd - response for transaction
        /// </summary>
        TransactionEnd = 0x23,

        /// <summary>
        /// Error - unrecognized message type
        /// <remarks>Shouldn't be send</remarks>
        /// </summary>
        Error = 0xFF,
    }
}
