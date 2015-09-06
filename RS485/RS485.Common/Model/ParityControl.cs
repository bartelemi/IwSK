using System.ComponentModel;

namespace RS485.Common.Model
{
    /// <summary>
    /// Type of transmission control
    /// </summary>
    public enum ParityControl
    {
        [Description("Brak")]
        None = 0,
        [Description("Bit nieparzystości")]
        Odd = 1,
        [Description("Bit parzystości")]
        Even = 2
    }
}