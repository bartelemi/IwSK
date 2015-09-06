using System.ComponentModel;

namespace RS485.Common.Model
{
    /// <summary>
    /// Number of stop bits after every character
    /// </summary>
    public enum StopBitsNumber
    {
        [Description("Jeden")]
        One = 1,
        [Description("Dwa")]
        Two = 2
    }
}