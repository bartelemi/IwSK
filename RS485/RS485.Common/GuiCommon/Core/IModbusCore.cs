using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;

namespace RS485.Common.GuiCommon.Core
{
    public interface IModbusCore
    {
        event LogMessageReceivedEventHandler LogMessageReceived;

        void ExecuteAction(ExecuteActionData actionData);
    }
}