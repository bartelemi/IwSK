using System;

namespace RS485.Common.GuiCommon.Models.EventArgs
{
    public delegate void LogMessageOccuredEventHandler(LogMessageOccuredEventArgs args);

    public class LogMessageOccuredEventArgs : System.EventArgs
    {
        public LogMessageType MessageType { get; private set; }
        public string Content { get; private set; }

        public LogMessageOccuredEventArgs(LogMessageType messageType, string content)
        {
            MessageType = messageType;
            Content = content;
        }

        public string ToReadableLogMessage()
        {
            return string.Format("{0:HH:mm:ss} - ###{1}### {2}", 
                DateTime.Now,
                MessageType.ToString().ToUpper(), 
                Content);
        }
    }
}