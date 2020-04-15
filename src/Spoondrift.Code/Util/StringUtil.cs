using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Util
{
    public static class StringUtil
    {
        public static string HexToString(string hex)
        {
            string s = "";
            if (hex != null)
            {
                for (int i = 0; i < hex.Split(',').Length; i++)
                {
                    try
                    {
                        // 每两个字符是一个 byte。 
                        s += Convert.ToChar(Convert.ToInt64(hex.Split(',')[i]));
                    }
                    catch
                    {
                        s = hex;
                    }
                }
            }
            return s;
        }
    }
}
