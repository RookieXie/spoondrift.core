using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Dapper
{
    public interface IUnitOfDapper
    {
        int RegisterSqlCommand(string sql,  DynamicParameters param);
        int RegisterStored(string storedName,  DynamicParameters param);
        T QueryObject<T>(string sqlString,  DynamicParameters cmdParms);
        object QueryObject(string sqlString,  DynamicParameters cmdParms);
        DataSet QueryDataSet(string sqlString,  DynamicParameters cmdParms);
        DataSet QueryDataSet(string sqlString);

        int Submit();
        //int EFSubmit();
        void AddUnitOfData(IUnitOfDapper unitOfData);

        List<IUnitOfDapper> UnitOfDataList { get; set; }
        DateTime Now { get; }

        string UnitSign { get; }
        string GetUniId();
        string GetSqlParamName(string param);
        int ExecuteSqlCommand(string sql,  DynamicParameters param);
        int ExecuteStored(string storedName,  DynamicParameters param);

        bool IsChildUnit { get; set; }
        //
        List<Func<IDbTransaction, int>> ApplyFun { get; set; }
    }
}
