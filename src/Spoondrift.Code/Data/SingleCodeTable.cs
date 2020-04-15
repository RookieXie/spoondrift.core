using Spoondrift.Code.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Spoondrift.Code.Util;
using Dapper;

namespace Spoondrift.Code.Data
{
    public abstract class SingleCodeTable<T> : CodeTable<T> where T : CodeDataModel
    {
        private string fWhere { get; set; }

        public abstract string RegName { get; set; }

        //public virtual AtawDbContext AtawDbContext { get; private set; }

        public abstract string TableName { get; set; }

        public abstract string TextField { get; set; }

        public abstract string ValueField { get; set; }

        public virtual List<string> OtherField { get; private set; }

        public virtual string Where { get; set; }

        public virtual string ModuleXml { get; set; }

        public override bool HasCache
        {
            get;
            set;
        }
        protected IServiceProvider provider;
        protected SingleCodeTable(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
            OtherField = new List<string>();
        }

        public IUnitOfDapper UnitOfData
        {
            get
            {
                return provider.GetService<IUnitOfDapper>();
            }
        }

        public override IEnumerable<T> BeginSearch(DataSet postDataSet, string key)
        {
            fWhere = string.Format(CultureInfo.CurrentCulture, "(  {0} LIKE '{1}%' OR {2} LIKE '%{1}' )", TextField, key, ValueField);
            return FillData(postDataSet);
            // Where = 
        }

        //public override IEnumerable<T> BeginSearch(DataSet postDataSet, string key, Pagination pagination)
        //{
        //    fWhere = string.Format(CultureInfo.CurrentCulture, "(  {0} LIKE '{1}%' OR {0} LIKE '%{1}%' )", TextField, key, ValueField);
        //    return FillData(postDataSet);
        //    // Where = 
        //}

        public override IEnumerable<T> Search(DataSet postDataSet, string key)
        {
            fWhere = string.Format(CultureInfo.CurrentCulture, "  {0} LIKE '%{1}%'", TextField, key);
            return FillData(postDataSet);
        }

        //public override IEnumerable<T> Search(DataSet postDataSet, string key, Pagination pagination)
        //{
        //    fWhere = string.Format(CultureInfo.CurrentCulture, "  {0} LIKE '%{1}%'", TextField, key);
        //    return FillData(postDataSet);
        //}
        public const string PAGE_SQL = "select CODE_VALUE, CODE_TEXT from ( "
                                   + "select row_number() over(order by {1} ) rn,{0} from {2} WHERE 1=1 {3})"
                                   + "tb where rn >(@PageNo)*@pageSize and rn <=(@PageNo + 1)*@pageSize";
        private IEnumerable<T> FillDataTop(string top)
        {
            //throw new NotImplementedException();
            //string sql = "SELECT " + top + " {0}  FROM {1}  WHERE {2}";
            string sql = "";
            if (PostDataSet != null)
            {
                sql = PAGE_SQL;
            }
            else
            {
                sql = " SELECT {0} FROM {2}  WHERE 1=1 {3}  ORDER BY {1}";
            }

            string select = " {0} AS CODE_VALUE , {1} AS CODE_TEXT  {2} ";


            string otherFields = string.Empty;
            if (OtherField.Count > 0)
            {
                otherFields = ", " + string.Join(",", OtherField);
            }

            select = string.Format(CultureInfo.CurrentCulture, select, ValueField, TextField, otherFields);

            string where = "  1 = 1 ";
            if (!string.IsNullOrEmpty(Where))
            {
                where = string.Format(CultureInfo.CurrentCulture, " {0} AND {1}", where, Where);
            }

            if (!string.IsNullOrEmpty(fWhere))
            {
                where = string.Format(CultureInfo.CurrentCulture, "{0} AND {1}", where, fWhere);
                // where += " AND " +  fWhere;
            }
            //else
            //    where = " 1=1 ";

            string orderName = ValueField + " DESC ";
            if (!where.TrimStart().StartsWith("AND"))
            {
                where = "AND ( {0} )".SFormat(where);
            }
            sql = sql.SFormat(select, orderName, TableName, where);

            // sql = string.Format(CultureInfo.CurrentCulture, sql, select, TableName, where);

            var dbContent = UnitOfData;
            //using (dbContent)
            //{
            // string sql = "SELECT ";

            DataSet ds = null;
            
            if (PostDataSet != null)
            {
                DataTable dt = PostDataSet.Tables[RegName + "_PAGER"];
                int pageNo = 0, pagesize = 28;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    pageNo = row["PageIndex"].Value<int>();
                    pagesize = row["PageSize"].Value<int>();
                }
                DynamicParameters paraList = new DynamicParameters();
                paraList.Add("@PageNo", pageNo );
                paraList.Add("@pageSize",pagesize);

                string countSql = string.Format(CultureInfo.CurrentCulture, "SELECT COUNT(*) FROM {0} WHERE  (1=1) {1}", TableName, where);
                int totalCount = dbContent.QueryObject(countSql, paraList).Value<int>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Rows[0]["TotalCount"] = totalCount;
                }
                ds = dbContent.QueryDataSet(sql, paraList);
            }
            else
            {
                ds = dbContent.QueryDataSet(sql);
            }


            var res = GetListByDs(ds);
            // Where = "";
            fWhere = "";
            return res.Cast<T>();
            // }
        }

        private List<CodeDataModel> GetListByDs(DataSet ds)
        {
            var list = new List<CodeDataModel>();
            DataTable dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                CodeDataModel model = new CodeDataModel
                {
                    CodeText = row["CODE_TEXT"].ToString(),
                    CodeValue = row["CODE_VALUE"].ToString()
                };
                list.Add(model);
            }

            return list;
        }

        public override IEnumerable<T> FillData(DataSet postDataSet)
        {
            return FillDataTop(" TOP 10 ");
            // return null;
        }

        //public override IEnumerable<T> FillData(DataSet postDataSet, Pagination pagination)
        //{
        //    return FillDataTop(" TOP 10 ");
        //    // return null;
        //}

        public override T this[string key]
        {
            get
            {

                // get
                // {
                string _classname = this.RegName.IsEmpty() ? (this.TableName + this.GetType().Name) : this.RegName;
                return Get(key);//可以添加缓存
            }

        }

        private T Get(string key)
        {
            fWhere = " {0} = '{1}' ".SFormat(ValueField, key);
            // Where = " " + ValueField + " = '" + key + "'";
            var res = FillData(null);
            T bean = res.FirstOrDefault();
            return bean;
        }

        public override IEnumerable<T> FillAllData(DataSet postDataSet)
        {
            return FillDataTop("100");
        }
    }
}
