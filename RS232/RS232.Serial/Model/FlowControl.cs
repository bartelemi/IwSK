using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Type of message flow control.
    /// <remarks>
    /// This type is mapped to System.IO.Ports.Handshake
    /// </remarks>
    /// </summary>
    public enum FlowControl
    {
        /// <summary>
        /// No control is used for the handshake.
        /// </summary>
        [Description("Brak")]
        None = 0,

        /// <summary>
        /// The XON/XOFF software control protocol is used. The XOFF control is sent
        /// to stop the transmission of data. The XON control is sent to resume the transmission.
        /// These software controls are used instead of Request to Send (RTS) and Clear
        /// to Send (CTS) hardware controls.
        /// </summary>
        [Description("Programowa")]
        Software = 1,

        /// <summary>
        /// Request-to-Send (RTS) hardware flow control is used. RTS signals that data
        /// is available for transmission. If the input buffer becomes full, the RTS
        /// line will be set to false. The RTS line will be set to true when more room
        /// becomes available in the input buffer.
        /// </summary>
        [Description("Sprzętowa")]
        Hardware = 2,

        /// <summary>
        /// Both the Request-to-Send (RTS) hardware control and the XON/XOFF software controls are used.
        /// </summary>
        [Description("Ręczna")]
        Manual = 10
    }
}