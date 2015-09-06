using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Master.Serial.Model
{
    class Transaction
    {
        private long startTime;

        private int durationInMs;

        public Transaction(int durationInMs)
        {
            startTime = DateTime.Now.Ticks;
            this.durationInMs = durationInMs;
        }

        public bool isTransactionStillActive()
        {
            return ((DateTime.Now.Ticks - startTime) / 10000) < durationInMs;
        }
    }
}
