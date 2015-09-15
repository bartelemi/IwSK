using System.ComponentModel;

namespace RS485.Common.Model
{

       /// <summary>
    /// type of transmission
    /// </summary>
    public enum TransactionType
    {
        [Description("Rozgłoszeniowa")]
        Broadcast = 1,
        [Description("Adresowa")]
        Address = 2
    }
    
}