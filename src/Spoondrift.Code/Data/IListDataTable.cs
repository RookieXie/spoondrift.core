using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Data
{
    public interface IListDataTable: IRegName, IPostDataResolver
    {
        FormConfig ModuleFormConfig { get; set; }

        IEnumerable<ObjectData> List { get; set; }

        void AppendTo(DataSet ds);

        //  void AppendToEmptyRow(DataSet ds);

        string PrimaryKey { get; set; }

        string ForeignKey { get; set; }

        // int PageSize { get; set; }
        void Initialize(ModuleFormInfo info);
        Pagination Pagination { get; set; }
        string KeyValue { get; set; }
        string ForeignKeyValue { get; set; }
        /// <summary>
        /// 没有查询条件的时候，是否返回空记录。默认是false 表示返回所有默认数据
        /// </summary>
        bool IsFillEmpty { get; set; }

        PageStyle PageStyle { get; set; }

        string Order { get; set; }

        DataFormConfig DataFormConfig { get; set; }

        //DataBaseConfig DataBase { get; }

        void SetPostDataRow(ObjectData data, DataAction dataAction, string key);

        string GetInsertKey(ObjectData data);

        

        //  event Func<ResponseResult, SubmitData> SubmitFilterEvent;
        //Func<SubmitData, SubmitData> SubmitFilterFun { get; }
    }
    public enum DataAction
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
    public class ModuleFormInfo
    {
        public FormConfig ModuleFormConfig { get; set; }
        public DataSet DataSet { get; set; }
        public int PageSize { get; set; }
        public string KeyValue { get; set; }
        public string ForeignKeyValue { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string ForeignKey { get; set; }
        public bool IsFillEmpty { get; set; }
        // public string DataXmlPath { get; set; }
        //public DataBaseConfig DataBase { get; set; }

        public string Order { get; set; }
        public DataFormConfig DataFormConfig { get; set; }
        public List<ColumnConfig> FullColumns { get; set; }

        public ModuleFormInfo(DataSet dataSet, int pageSize, string keyValue, string foreignKeyValue,
            string tableName, string primaryKey, string foreignKey, bool isFillEmpty, DataFormConfig dataFormConfig,
            string order,  FormConfig moduleFormConfig)
        {
            DataSet = dataSet;
            PageSize = pageSize;
            KeyValue = keyValue;
            ForeignKey = foreignKey;
            ForeignKeyValue = foreignKeyValue;
            TableName = tableName;
            PrimaryKey = primaryKey;
            ForeignKey = foreignKey;
            IsFillEmpty = isFillEmpty;
            DataFormConfig = dataFormConfig;
            Order = order;
            ModuleFormConfig = moduleFormConfig;
        }
    }
}
