using RS485.Master.Serial.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Master.Serial.Implementation
{
    class FrameBuilder
    {
        public Frame buildFrame(string deviceAddress, string message)
        {
            string LRC = calculateLRC(deviceAddress + message);
            return new Frame(deviceAddress, message, LRC);
        }

        private string calculateLRC(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] rawOutputLrc = new byte[1];
            rawOutputLrc[0] = calculateLRConRawData(inputBytes);
            return Encoding.ASCII.GetString(rawOutputLrc);
        }
        private static byte calculateLRConRawData(byte[] bytes)
        {
            byte LRC = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                LRC ^= bytes[i];
            }
            return LRC;
        }
    }
}
