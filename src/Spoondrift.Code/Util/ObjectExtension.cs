using Spoondrift.Code.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        public static string SFormat(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
        public static bool IsNotEmpty<T>(this IList<T> list) where T : class
        {
            return list != null && list.Count > 0;
        }
        public static void SetDataRowValue(this ObjectData od, string colName, object val)
        {
            if (od.MODEFY_COLUMNS != null && !od.MODEFY_COLUMNS.Contains(colName))
            {
                od.MODEFY_COLUMNS.Add(colName);
            }

            if (!od.Row.Table.Columns.Contains(colName))
            {
                od.Row.Table.Columns.Add(colName);
            }
            od.Row[colName] = val;
        }
        public static void ObjectClone<T>(this T source, T t)
        {
            foreach (var pS in source.GetType().GetProperties())
            {
                foreach (var pT in t.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name) continue;
                    (pT.GetSetMethod()).Invoke(t, new object[] { pS.GetGetMethod().Invoke(source, null) });
                }
            };
        }
        public static object GetDataRowValue(this ObjectData od, string colName)
        {
            if (od.MODEFY_COLUMNS.Contains(colName))
            {
                if (od.Row.Table.Columns.Contains(colName))
                {
                    return od.Row[colName];
                }
            }
            return null;
        }
    }
}
