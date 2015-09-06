using RS485.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Implementation
{
    public class FrameBuilder
    {
        public static Frame buildFrame(string deviceAddress, string message)
        {
            string LRC = calculateLRC(deviceAddress + message);
            return new Frame(deviceAddress, message, LRC);
        }

        private static string calculateLRC(string input)
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
