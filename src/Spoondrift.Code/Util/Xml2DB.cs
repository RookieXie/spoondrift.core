using Dapper;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.Util
{
    public class Xml2DB
    {
        private List<string> tableNames;
        private readonly IUnitOfDapper _unitOfDapper;
        public Xml2DB(IUnitOfDapper unitOfDapper)
        {
            _unitOfDapper = unitOfDapper;
            tableNames = new List<string>();
            string sql = "select table_name from information_schema.TABLES where TABLE_SCHEMA=(select database());";
            DataSet dataSet = _unitOfDapper.QueryDataSet(sql);
            
            DataRowCollection dataRows = dataSet.Tables[0].Rows;
            for (int i = 0; i < dataRows.Count; i++)
            {
                tableNames.Add(dataRows[i][0].ToString().Trim().ToUpper());
            }
        }
        public void Migrations(DataFormConfig dataFormConfig)
        {
            DynamicParameters sqlParameters = new DynamicParameters();
            //dataFormConfig.AddBaseColumns("BaseForm.xml");
            if (dataFormConfig == null) { return; }
            if (dataFormConfig.TableName.IsEmpty()) dataFormConfig.TableName = dataFormConfig.Name;
            if (dataFormConfig.TableName.ToUpper().IndexOf("VIEW") == 0) { return; }//若是试图，不执行任何操作
            if (HaveTable(dataFormConfig.TableName))
            {
                List<string> tableFields = GetTableFields(dataFormConfig.TableName);
                string sql = string.Empty;
                string commentSql = string.Empty;
                //SqlParameter[] sqlParameters;
                foreach (var column in dataFormConfig.Columns)
                {
                    if (column.Kind == ColumnKind.Data && column.SourceName.IsEmpty())
                    {
                        if (tableFields.FirstOrDefault<string>(n => n == column.Name.Trim().ToUpper()) == default(string))
                        {

                            sql = string.Format(CultureInfo.CurrentCulture, "{0} ALTER TABLE {1} Add {2} {3} COMMENT '{4}' ", sql, dataFormConfig.TableName, column.Name, ConvertToDataBaseType(column), column.DisplayName);
                            //if (column.IsKey)
                            //{
                            //    sql += $" PRIMARY KEY({column.Name})";
                            //}
                            sql = $"{sql};";
                        }
                        //sqlParameters = new SqlParameter[]
                        //{
                        //    new SqlParameter("@tableName",dataFormConfig.TableName),
                        //    new SqlParameter("@fieldName",column.Name),
                        //    new SqlParameter("@fieldType",ConvertToDataBaseType(column.ControlType,column.Length))
                        //};
                    }
                }
                if (!string.IsNullOrEmpty(sql))
                {
                    _unitOfDapper.RegisterSqlCommand(sql, sqlParameters);
                    //_unitOfDapper.RegisterSqlCommand(commentSql,sqlParameters);
                    //this.IsMigrations = true;
                    //logMessages.Add(new SqlLogMessage { SqlStr = sql });
                }
            }
            else
            {
                string commentSql = string.Empty;
                string sql = "CREATE TABLE " + dataFormConfig.TableName + " (";
                foreach (var column in dataFormConfig.Columns)
                {
                    if (column.Kind == ColumnKind.Data)
                    {
                        if (column.IsKey)
                        {
                            sql = string.Format(CultureInfo.CurrentCulture, "{0} {1} {2}  COMMENT '{3}',", sql, column.Name, ConvertToDataBaseType(column), column.DisplayName);
                            sql = $"{sql} PRIMARY KEY({column.Name}),";
                        }
                        else
                        {
                            sql = string.Format(CultureInfo.CurrentCulture, "{0} {1} {2} COMMENT '{3}',", sql, column.Name, ConvertToDataBaseType(column), column.DisplayName);
                        }
                    }
                }
                sql = sql.Substring(0, sql.Length - 1) + ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
               
                _unitOfDapper.RegisterSqlCommand(sql, sqlParameters);
                //this.IsMigrations = true;
                //logMessages.Add(new SqlLogMessage { SqlStr = sql });
            }
            _unitOfDapper.Submit();
        }
        private bool HaveTable(string tableName)
        {
            return tableNames.FirstOrDefault<string>(n => n == tableName.Trim().ToUpper()) != default(string);
        }
        private List<string> GetTableFields(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName不能为空");
            }
            List<string> tableFields = new List<string>();
            string sql = "select column_name from information_schema.COLUMNS where TABLE_SCHEMA = (select database()) and TABLE_NAME=@tableName";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@tableName", tableName);
            DataSet dataSet = _unitOfDapper.QueryDataSet(sql, sqlParameters);
            //logMessages.Add(new SqlLogMessage { SqlStr = sql, Parameters = sqlParameters });
            DataRowCollection dataRows = dataSet.Tables[0].Rows;
            for (int i = 0; i < dataRows.Count; i++)
            {
                tableFields.Add(dataRows[i][0].ToString().Trim().ToUpper());
            }
            return tableFields;
        }
        private string ConvertToDataBaseType(ColumnConfig column)
        {
            int length = column.Length == 0 ? 200 : column.Length;
            if (column.DataType != default(XmlDataType))
            {
                switch (column.DataType)
                {
                    case XmlDataType.String:
                        return "NVARCHAR(" + length + ")";
                    case XmlDataType.Binary:
                        return "VARBINARY(" + length + ")";
                    case XmlDataType.Bit:
                        return "BIT";
                    case XmlDataType.Byte:
                        return "BLOB";
                    case XmlDataType.Date:
                        return "DATETIME";
                    case XmlDataType.DateTime:
                        return "DATETIME";
                    case XmlDataType.Decimal:
                        return "DECIMAL";
                    case XmlDataType.Double:
                        return "FLOAT";
                    case XmlDataType.Int:
                        return "INT";
                    case XmlDataType.Long:
                        return "BIGINT";
                    case XmlDataType.Money:
                        return "numeric(18,4)";
                    case XmlDataType.Short:
                        return "SMALLINT";
                    case XmlDataType.Text:
                        return "TEXT";
                    default:
                        return "VARCHAR(200)";
                }
            }
            else if (column.ControlType != ControlType.None)
            {
                ControlType controlType = column.ControlType;
                if (controlType == ControlType.Radio || controlType == ControlType.Combo || controlType == ControlType.CheckBox)
                {
                    return "INT";
                }
                if (controlType == ControlType.Date || controlType == ControlType.DateTime || controlType == ControlType.DetailDate)
                {
                    return "DATETIME";
                }
                if (controlType == ControlType.TextArea || controlType == ControlType.MultiSelector || controlType == ControlType.FormMultiSelector)
                {
                    return "TEXT";
                }
                if (controlType == ControlType.SingleFileUpload || controlType == ControlType.SingleImageUpload
                    || controlType == ControlType.MultiFileUpload || controlType == ControlType.MultiImageUpload || controlType == ControlType.ImageDetail)
                {
                    return "TEXT";
                }
                else
                {
                    return "VARCHAR(" + length + ")";
                }
            }
            return "VARCHAR(" + length + ")";
        }
    }
}
