using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Data
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class Pagination
    {

        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// 分页索引 从0开始
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int TotalCount
        {
            get;
            set;
        }
        public string SortName
        {
            get;
            set;
        }
        /// <summary>
        /// 默认值是false表示 倒序排列
        /// </summary>
        public bool IsASC
        {
            get;
            set;
        }

        public DateTime DataTime
        {
            get;
            set;
        }

        private T RowValue<T>(DataRow row, string name)
        {
            if (row.Table.Columns.Contains(name))
            {
                return row[name].Value<T>();
            }
            return default(T);
        }

        public Pagination FormDataTable(DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                this.PageSize = RowValue<int>(row, "PageSize");
                this.PageIndex = RowValue<int>(row, "PageIndex");

                this.IsASC = RowValue<bool>(row, "IsASC");
                //row["IsASC"].Value<bool>();
                this.SortName = RowValue<string>(row, "SortName");
                // this.TotalCount = row["TotalCount"].Value<int>();
                this.DataTime = RowValue<DateTime>(row, "DataTime");
            }


            return this;
        }

        public DataTable AppendToDataSet(DataSet dataSet, string tableName)
        {
            string _tableName = tableName + "_PAGER";
            DataTable dt = null;
            if (!dataSet.Tables.Contains(_tableName))
            {
                dt = new DataTable(_tableName);
                dt.Columns.Add("TableName", typeof(string));
                dt.Columns.Add("PageSize", typeof(int));
                dt.Columns.Add("PageIndex", typeof(int));
                dt.Columns.Add("IsASC", typeof(bool));
                dt.Columns.Add("SortName");
                dt.Columns.Add("TotalCount", typeof(int));
                dt.Columns.Add("DataTime", typeof(DateTime));
                dataSet.Tables.Add(dt);
            }
            else
                dt = dataSet.Tables[_tableName];
            DataRow row = dt.NewRow();
            row.BeginEdit();
            row["TableName"] = tableName;
            row["PageSize"] = this.PageSize;
            row["PageIndex"] = this.PageIndex;
            row["IsASC"] = this.IsASC;
            row["SortName"] = this.SortName;
            row["TotalCount"] = this.TotalCount;
            row["DataTime"] = this.DataTime;

            row.EndEdit();
            dt.Rows.Add(row);

            return dt;
        }
    }
}
