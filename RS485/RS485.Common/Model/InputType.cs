using System.ComponentModel;

namespace RS485.Common.Model
{
    /// <summary>
    /// Input type for messages
    /// </summary>
    public enum InputType
    {
        [Description("Binarna")]
        Binary,
        [Description("Tekstowa")]
        Text
    }
}