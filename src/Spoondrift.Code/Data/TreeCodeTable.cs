using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Data
{
    public abstract class TreeCodeTable : CodeTable<CodeDataModel>
    {
        public TreeCodeTable()
        {
            HasCache = false;
        }

        public virtual string Root
        {
            get { return "0"; }
        }

        public virtual bool OnlyLeafCheckbox { get; set; }
        public override bool HasCache
        {
            get;
            set;
        }

        public abstract TreeCodeTableModel GetDisplayTreeNode(string key);
        public abstract TreeCodeTableModel GetChildrenNode(string key);
        public abstract TreeCodeTableModel GetAllTree();
        public abstract IEnumerable<CodeDataModel> GetDescendent(string key);

        public override CodeDataModel this[string key]
        {
            get { throw new System.NotImplementedException(); }
        }

        public override IEnumerable<CodeDataModel> FillData(System.Data.DataSet postDataSet)
        {
            throw new System.NotImplementedException();
        }

        //public override IEnumerable<CodeDataModel> FillData(System.Data.DataSet postDataSet, Pagination pagination = null)
        //{
        //    throw new System.NotImplementedException();
        //}

        public override IEnumerable<CodeDataModel> BeginSearch(System.Data.DataSet postDataSet, string key)
        {
            throw new System.NotImplementedException();
        }

        //public override IEnumerable<CodeDataModel> BeginSearch(System.Data.DataSet postDataSet, string key, Pagination pagination = null)
        //{
        //    throw new System.NotImplementedException();
        //}

        public override IEnumerable<CodeDataModel> Search(System.Data.DataSet postDataSet, string key)
        {
            throw new System.NotImplementedException();
        }

        //public override IEnumerable<CodeDataModel> Search(System.Data.DataSet postDataSet, string key, Pagination pagination = null)
        //{
        //    throw new System.NotImplementedException();
        //}

        public override IEnumerable<CodeDataModel> FillAllData(System.Data.DataSet postDataSet)
        {
            throw new System.NotImplementedException();
        }
    }
}
