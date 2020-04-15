using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Dapper
{
    public class DbTrans
    {
        public string Sql { get; set; }
        public CommandType CommandType { get; set; }
        public DynamicParameters Param { get; set; }
        public DataSet QueryDataSet(IUnitOfDapper unitData)
        {
            //if (ParamList != null && ParamList.Count > 0)
            //{
            return unitData.QueryDataSet(Sql, Param);
            //}
            //return null;
        }

        public object QueryObject(IUnitOfDapper unitData)
        {

            return unitData.QueryObject(Sql, Param);
        }
    }
}
