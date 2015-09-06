using System.ComponentModel;

namespace RS485.Common.Model
{
    /// <summary>
    /// Type of terminator, sequence of characters ending each message
    /// </summary>
    public enum TerminatorType
    {
        [Description("Brak")]
        None,
        [Description("Carriage Return")]
        CR,
        [Description("Line Feed")]
        LF,
        [Description("CRLF")]
        CRLF,
        [Description("Własny")]
        Custom
    }
}