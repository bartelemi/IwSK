using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Current state of connection
    /// </summary>
    public enum ConnectionState
    {
        [Description("Automatyczne konfigurowanie. . .")]
        Autobauding,
        [Description("Połączono")]
        Connected,
        [Description("Ustanawianie połączenia. . .")]
        Connecting,
        [Description("Rozłączono")]
        Disconnected,
        [Description("Błąd")]
        Error
    }
}