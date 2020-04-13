using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class HashCodeTable : CodeTable<CodeDataModel>
    {
        protected virtual Dictionary<string, HashCodeDataModel> DataList { get; set; }
        public override CodeDataModel this[string key]
        {
            get
            {
                if (DataList.ContainsKey(key))
                {
                    return DataList[key];
                }
                else
                    return null;

            }
        }

        public override bool HasCache
        {
            get;
            set;
        }

        public override IEnumerable<CodeDataModel> BeginSearch(DataSet postDataSet, string key)
        {
            var res = DataList.Where(a => a.Value.CODE_TEXT.IndexOf(key) == 0).Select(a => a.Value);
            List<HashCodeDataModel> list = new List<HashCodeDataModel>();
            list.AddRange(res);
            return list;
        }

        public override IEnumerable<CodeDataModel> FillAllData(DataSet postDataSet)
        {
            return FillData(postDataSet);
           
        }

        public override IEnumerable<CodeDataModel> FillData(DataSet postDataSet)
        {
            foreach (var item in DataList)
            {
                yield return item.Value;
            }
        }

        public override IEnumerable<CodeDataModel> Search(DataSet postDataSet, string key)
        {
            var res = DataList.Where(a => a.Value.CODE_TEXT.Contains(key)).Select(a => a.Value);
            List<HashCodeDataModel> list = new List<HashCodeDataModel>();
            list.AddRange(res);
            return list;
        }
    }
    public class HashCodeDataModel : CodeDataModel
    {
        public string CODE_NAME { get; set; }
    }
}
