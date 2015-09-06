using System.Text;

namespace RS485.Common.Converters
{
    public class StringToVisibleASCIIConverter
    {
        public string Convert(string value)
        {
            if (value == null)
                return string.Empty;

            var sb = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                int c = value[i];
                if(c < 32 || c == 127)
                {
                    sb.Append(string.Format("\\c{0} ", c));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }

        public string ConvertBack(string value, int trim = 0)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var sb = new StringBuilder();

            for (int i = 1; i <= value.Length; i++)
            {
                char c = value[i - 1];

                if (c == '\\' && value[i] != '\\')
                {
                    var number = new StringBuilder();
                    while (i < value.Length && value[i] != ' ')
                    {
                        char cc = value[i];
                        if (char.IsDigit(cc))
                        {
                            number.Append(cc);    
                        }
                        else
                        {
                            return string.Empty;
                        }
                        i++;
                    }

                    int n = int.Parse(number.ToString());
                    if (n < 0 || n > 127)
                    {
                        c = '\0';
                    }
                    else
                    {
                        c = (char)n;
                    }

                    if (i < value.Length && value[i] == ' ')
                        ++i;
                }

                sb.Append(c);
            }

            string result = sb.ToString();
            if (trim > 0 && result.Length > trim)
            {
                result.Substring(0, 2);
            }

            return result;
        }
    }
}