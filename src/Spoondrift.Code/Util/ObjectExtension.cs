using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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
        public static string RemoveEnd(this string str, string end)
        {
            if (str.Length >= end.Length)
            {
                var _index = str.LastIndexOf(end);
                if (_index >= 0)
                {
                    return str.Remove(_index);
                }
            }
            return str;
        }
        public static T Value<T>(this object objValue)
        {
            if (objValue == null || objValue == DBNull.Value)
                return default(T);
            Type destType = typeof(T);
            if (objValue.GetType() == destType)
                return (T)objValue;
            try
            {
                return (T)System.Convert.ChangeType(objValue, destType);
            }
            catch
            {
                return default(T);
            }
        }
        public static string GetDescription(this object obj)
        {
            bool isTop = false;
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                       fi, typeof(DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
            }
            return obj.ToString();
        }
    }
}
