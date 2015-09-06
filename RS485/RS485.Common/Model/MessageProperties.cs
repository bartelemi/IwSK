namespace RS485.Common.Model
{
    /// <summary>
    /// Represents message parameters
    /// </summary>
    public class MessageProperties
    {
        public MessageType MessageType { get; set; }
        public bool AppendDateTime { get; set; }
        public string TerminalString { get; set; }
    }
}