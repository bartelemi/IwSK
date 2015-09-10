using RS485.Common.Exceptions;
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

        public static Frame mapFromString(string frame)
        {
            String[] splitted = frame.Split(Frame.DATA_SEPARATOR.ToCharArray());
            if (splitted.Length != 3)
            {
                throw new InvalidOperationException("Cannot read frame from string: " + frame);
            }
            string deviceAddress = splitted[0];
            string message = splitted[1];
            string lrc_frame = splitted[3];
            string lrc_calculated = calculateLRC(deviceAddress + message);
            if (!lrc_frame.Equals(lrc_calculated))
            {
                throw new InvalidChecksumException(lrc_frame + "!=" + lrc_calculated); 
            }
            return new Frame(deviceAddress, message, lrc_calculated);
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
