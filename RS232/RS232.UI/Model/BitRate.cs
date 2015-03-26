using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.UI.Model
{
    /// <summary>
    /// Specifies transmission speed in bits per second
    /// </summary>
    public enum BitRate
    {
        [Description("75 bps")]
        BR_75 = 75,
        [Description("110 bps")]
        BR_110 = 110,
        [Description("300 bps")]
        BR_300 = 300,
        [Description("1200 bps")]
        BR_1200 = 1200,
        [Description("2400 bps")]
        BR_2400 = 2400,
        [Description("4800 bps")]
        BR_4800 = 4800,
        [Description("9600 bps")]
        BR_9600 = 9600,
        [Description("19200 bps")]
        BR_19200 = 19200,
        [Description("38400 bps")]
        BR_38400 = 38400,
        [Description("57600 bps")]
        BR_57600 = 57600,
        [Description("115200 bps")]
        BR_115200 = 115200,
    }
}
