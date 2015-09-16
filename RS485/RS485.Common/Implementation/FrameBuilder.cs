using RS485.Common.Converters;
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
            if (frame.Length < 5)
            {
                throw new InvalidOperationException("Cannot read frame from string: " + frame + " too short frame");
            }
            string deviceAddress = frame.Substring(0, 3);
            string message = frame.Substring(3, frame.Length - 5);
            string lrc_frame = frame.Substring(frame.Length - 2, 2);
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
                    byte rawOutputLrc = calculateLRConRawData(inputBytes);
                    return ToHexConverter.GetHexForm(rawOutputLrc);
                }

          private static byte calculateLRConRawData(byte[] bytes)
          {
              byte LRC = 0;
              for (int i = 0; i < bytes.Length; i++)
              {
                  LRC += bytes[i];
              }
              LRC = (byte)(0xFF - LRC);
              LRC += 1;
              return LRC;
          }
    }
       
    }
