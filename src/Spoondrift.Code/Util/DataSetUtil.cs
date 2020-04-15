using Spoondrift.Code.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Spoondrift.Code.Util
{
    public static class DataSetUtil
    {
        /// <summary>
        /// 将实体类转换成DataTable 
        /// </summary>
        /// <param name="objlist"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> objlist)
        {
            if (objlist == null || objlist.Count <= 0)
            {
                return null;
            }
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties();

            foreach (var t in objlist)
            {
                if (t == null)
                {
                    continue;
                }

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {

                        Type type = pi.PropertyType;
                        if (IsNullableType(type))
                            column = new DataColumn(name, type.GetGenericArguments()[0]);
                        else
                            column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null) ?? DBNull.Value;
                }

                dt.Rows.Add(row);
            }
            return dt;
        }
        private static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }
        public static List<ObjectData> FillModel(DataTable dt, Type objType, HashSet<string> xmlColumns)
        {

            List<ObjectData> l = new List<ObjectData>();
            bool isIObjectData = false;
           
            if (dt.Columns[0].ColumnName == "rowId")
            {
                dt.Columns.Remove("rowId");
            }
            foreach (DataRow dr in dt.Rows)
            {
                Object model = Activator.CreateInstance(objType);
                var _objectData = model as ObjectData;
                if (_objectData != null)
                {
                    isIObjectData = true;
                    _objectData.MODEFY_COLUMNS = new HashSet<string>();
                    _objectData.Row = dr;
                }

                foreach (DataColumn dc in dr.Table.Columns)
                {

                    PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName);
                    if (pi != null)
                    {
                        if (dr[dc.ColumnName] != DBNull.Value)
                        {
                            var ff = pi.ReflectedType;
                            var _obj = Convert.ChangeType(dr[dc.ColumnName].ToString(), pi.PropertyType); //XmlUtil.GetValue(dr, pi.PropertyType, dr[dc.ColumnName].ToString());
                            pi.SetValue(model, _obj, null);
                            if (isIObjectData)
                            {
                                // Attribute.GetCustomAttribute(type, typeof(CodePlugAttribute)) as CodePlugAttribute
                                _objectData.MODEFY_COLUMNS.Add(dc.ColumnName);

                            }

                        }
                        else
                        {
                            pi.SetValue(model, null, null);
                        }
                    }
                    else
                    {
                        if (xmlColumns != null)
                        {
                            if (xmlColumns.Contains(dc.ColumnName) && dr[dc.ColumnName] != DBNull.Value)
                            {
                                _objectData.MODEFY_COLUMNS.Add(dc.ColumnName);
                            }
                        }
                    }

                }
                l.Add(_objectData);
            }

            return l;
        }
        public static List<T> FillModel<T>(DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(DataRoToEntityIgnoreCase<T>(dt.Columns, dr));
                }
            }
            return list;
        }
        private static T DataRoToEntityIgnoreCase<T>(DataColumnCollection colums, DataRow row) where T : class, new()
        {
            T t = new T();
            foreach (DataColumn c in colums)
            {
                string value = row[c.ColumnName].ToString();
                //忽略大小写
                BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
                var type = typeof(T);
                var property = type.GetProperty(c.ColumnName, flag);
                if (property == null)
                {
                    continue;
                }
                //property.SetValue(t, Convert.ChangeType(value, property.PropertyType),null);
                if (!property.PropertyType.IsGenericType)
                {
                    //非泛型
                    property.SetValue(t, string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, property.PropertyType), null);
                }
                else
                {
                    //泛型Nullable<>
                    Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        property.SetValue(t, string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)), null);
                    }
                }

            }
            return t;
        }
        public static void MegerDataTable(DataTable from, DataTable to, string keyColName)
        {
            foreach (DataColumn col in from.Columns)
            {
                string colName = col.ColumnName;
                if (!(to.Columns.Contains(colName)))
                {
                    to.Columns.Add(colName, col.DataType);
                }
            }

            foreach (DataRow row in from.Rows)
            {
                string key = row[keyColName].Value<string>();

                DataRow toRow = null;

                foreach (DataRow _row in to.Rows)
                {
                    if (_row[keyColName].Value<string>() == key && !key.IsEmpty())
                    {
                        toRow = _row;
                        break;
                    }
                }

                if (toRow == null)
                {
                    toRow = to.NewRow();
                    to.Rows.Add(toRow);
                }
                toRow.BeginEdit();
                foreach (DataColumn col in from.Columns)
                {
                    string colName = col.ColumnName;
                    toRow[colName] = row[colName];
                }
                toRow.EndEdit();
            }



        }
    }
}
