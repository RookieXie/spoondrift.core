using Spoondrift.Code.Config;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class DecodeOptionCreator : BaseOptionCreator
    {
        protected IServiceProvider provider;
        public DecodeOptionCreator(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public override BaseOptions Create()
        {
            var options = base.Create();
            // string codeTableName = BaseOptions.RegName;
            string codeTableName = this.Config.DetailRegName.IsEmpty() ? options.RegName : this.Config.DetailRegName;
            if (codeTableName.IsEmpty())
                return options;
            string colName = this.Config.Name;
            string tableName = this.FormView.TableName;
            var ds = this.PageView.Data;
            PreDecode(tableName, codeTableName, colName, ds);
            return options;
        }

        protected virtual void PreDecode(string tableName, string codeTableName, string colName, DataSet ds)
        {
            string text = "";
            string insertTableName = tableName + "_INSERT";
            string codeColumn = colName + "_CODEINDEX";//索引列，用于查找CodeValue在CodeTable中的CodeText值
            switch (ViewPageStyle)
            {
                case PageStyle.Insert:
                    if (ds.Tables.Contains(insertTableName))
                    {
                        //新增时，该选择器控件存在默认值时，才需要新增索引列
                        if (!ds.Tables[insertTableName].Columns.Contains(codeColumn) &&
                            ds.Tables[insertTableName].Columns.Contains(colName))
                        {
                            ds.Tables[insertTableName].Columns.Add(codeColumn, typeof(int[]));
                        }
                        if (ds.Tables[insertTableName].Columns.Contains(codeColumn))
                            ds.Tables[insertTableName].Rows[0][codeColumn] = Decode(this.Config.DefaultValueStr, codeTableName, out text);
                    }
                    break;
                case PageStyle.List:
                case PageStyle.Detail:
                //if (ds.Tables.Contains(tableName))
                //{
                //    foreach (DataRow row in ds.Tables[tableName].Rows)
                //    {
                //        string val = row[colName].ToString();
                //        Decode(val, codeTableName, out text);
                //        row[colName] = text;
                //    }
                //}
                //break;
                case PageStyle.Update:
                    if (ds.Tables.Contains(tableName))
                    {
                        if (!ds.Tables[tableName].Columns.Contains(codeColumn))
                            ds.Tables[tableName].Columns.Add(codeColumn, typeof(int[]));
                        foreach (DataRow row in ds.Tables[tableName].Rows)
                        {
                            if (row.Table.Columns.Contains(colName))
                            {
                                string val = row[colName].ToString();
                                row[codeColumn] = Decode(val, codeTableName, out text);
                            }
                        }
                    }
                    break;

            }

        }
        /// <summary>
        /// CodeTable解码,返回CodeValue在Codetable中的索引值数组，不存在返回只有-1的数组
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private int[] Decode(string val, string codeTableName, out string text)
        {
            text = "";
            if (!val.IsEmpty())
            {
                string[] arr = val.Split(',');
                int[] result = new int[arr.Length];
                var codeTable = provider.GetCodePlugService<CodeTable<CodeDataModel>>(codeTableName);// codeTableName.CodePlugIn<CodeTable<CodeDataModel>>();
                string arrStr = "";
                string dataText = "";
                for (int i = 0; i < result.Length; i++)
                {
                    arrStr = arr[i].Replace("\"", "").Replace("'", "");
                    if (codeTable!=null && codeTable[arrStr] != null)
                    {
                        dataText = codeTable[arrStr].CodeText;
                        text = text + dataText + ",";
                    }
                    else
                    {
                        result[i] = -1;
                        text = text + ",";
                        continue;
                    }
                    int index = 0;
                    if (this.PageView.Data.Tables.Contains(BaseOptions.RegName))
                    {
                        var dt = this.PageView.Data.Tables[BaseOptions.RegName];
                        var dataRow = dt.Rows.Find(arrStr);
                        if (dataRow == null)
                        {
                            var dr = dt.NewRow();
                            dr.BeginEdit();
                            dr["CODE_VALUE"] = arrStr;
                            dr["CODE_TEXT"] = dataText;
                            dr["CODE_INDEX"] = index = dt.Rows.Count;
                            dr.EndEdit();
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            index = dataRow["CODE_INDEX"].Value<int>();
                        }
                    }
                    else
                    {
                        var newTable = new DataTable(BaseOptions.RegName);
                        newTable.Columns.Add("CODE_VALUE");
                        newTable.Columns.Add("CODE_TEXT");
                        newTable.Columns.Add("CODE_INDEX", typeof(int));
                        newTable.PrimaryKey = new DataColumn[] { newTable.Columns["CODE_VALUE"] };
                        var dr = newTable.NewRow();
                        dr.BeginEdit();
                        dr["CODE_VALUE"] = arrStr;
                        dr["CODE_TEXT"] = dataText;
                        dr["CODE_INDEX"] = 0;
                        dr.EndEdit();
                        newTable.Rows.Add(dr);
                        this.PageView.Data.Tables.Add(newTable);
                    }
                    result[i] = index;
                }
                //if (fSelectorOptions.DataText == null)
                //{
                //    fSelectorOptions.DataText = new JsDataValue()
                //     {
                //         ColumnName = "CODE_TEXT",
                //         DataValueType = JsDataValueType.Table,
                //         TableName = tableName
                //     };
                //}
                if (!text.IsEmpty())
                    text = text.Remove(text.Length - 1);
                return result;
            }
            return new int[] { -1 };
        }
    }
}
