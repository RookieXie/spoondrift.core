using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Util
{
    public static class ObjectExtension
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
