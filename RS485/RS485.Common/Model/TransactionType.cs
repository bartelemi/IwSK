using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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