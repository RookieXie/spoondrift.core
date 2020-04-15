using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Data
{
    public abstract class CodeTable<T> : IDisposable, IRegName
    {
        public string CodePlugName { get; set; }
        public virtual string Param
        {
            get;
            set;
        }

        public virtual string Sign
        {
            get;
            set;
        }

        public virtual string DataBase
        {
            get;
            set;
        }

        public virtual string Product
        {
            get;
            set;
        }

        protected DataSet PostDataSet
        {
            get;
            set;
        }

        public IEnumerable<string> KeyValues
        {
            get;
            set;
        }

        public void Initialize(DataSet postDataSet)
        {
            PostDataSet = postDataSet;
            if (postDataSet != null && postDataSet.Tables.Count > 0)
            {
                DataTable dt = postDataSet.Tables["_OPERATION"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> keyValueList = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string _key = row["KeyValue"].ToString();
                        keyValueList.Add(_key);
                    }
                    KeyValues = keyValueList;
                }
            }
        }

        public abstract T this[string key] { get; }

        public abstract bool HasCache { get; set; }

        public abstract IEnumerable<T> FillData(DataSet postDataSet);

        //public abstract IEnumerable<T> FillData(DataSet postDataSet, Pagination pagination = null);

        public abstract IEnumerable<T> BeginSearch(DataSet postDataSet, string key);

        //public abstract IEnumerable<T> BeginSearch(DataSet postDataSet, string key,  Pagination pagination = null);

        public abstract IEnumerable<T> Search(DataSet postDataSet, string key);

        //public abstract IEnumerable<T> Search(DataSet postDataSet, string key, Pagination pagination = null);

        public abstract IEnumerable<T> FillAllData(DataSet postDataSet);

        public void Dispose()
        {
            if (PostDataSet != null)
                PostDataSet.Dispose();
            // throw new System.NotImplementedException();
        }
    }
}
