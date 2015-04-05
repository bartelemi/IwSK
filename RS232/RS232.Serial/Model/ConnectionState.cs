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
        [Description("Automatyczne konfigurowanie połączenia. . .")]
        Autobauding,
        [Description("Ustanawianie połączenia. . .")]
        Connecting,
        [Description("Połączono")]
        Connected,
        [Description("Rozłączam. . .")]
        Disconnecting,
        [Description("Rozłączono")]
        Disconnected,
        [Description("Wysyłanie")]
        Sending,
        [Description("Błąd")]
        Error
    }
}