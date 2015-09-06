using System.ComponentModel;

namespace RS485.Common.Model
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