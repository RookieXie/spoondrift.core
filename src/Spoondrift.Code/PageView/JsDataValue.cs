using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class JsDataValue
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public int Index { get; set; }

        public string FunString { get; set; }

        public JsDataValueType DataValueType { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 新增，有默认值时，该值为true
        /// </summary>
        public bool IsChange { get; set; }

        public JsDataValue()
        {
        }

        public JsDataValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Value = value;
                DataValueType = JsDataValueType.ValueObj;
            }
            else
            {
                DataValueType = JsDataValueType.None;
            }
        }

        public JsDataValue(string tableName, string columnName)
        {
            TableName = tableName;
            ColumnName = columnName;
            DataValueType = JsDataValueType.Table;
        }

    }
}
