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
        public static float lengToFloat(this string str)
        {
            if (!str.IsEmpty())
            {
                str = str.ToUpper();
                if (str.LastIndexOf("PX") == (str.Length - 2))
                {
                    string _str = str.RemoveEnd("PX");
                    return _str.Value<float>();
                }
                if (str.LastIndexOf("REM") == (str.Length - 3))
                {
                    string _str = str.RemoveEnd("REM");
                    var _re = _str.Value<float>();
                    return _re * 14;
                }


            }
            return 0;
        }
    }
}
