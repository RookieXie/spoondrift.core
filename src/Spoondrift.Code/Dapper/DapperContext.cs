using Dapper;
using MySql.Data.MySqlClient;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.Dapper
{
    public class DapperContext : IUnitOfDapper
    {
        private readonly string _connectionString;
        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(_connectionString);
            }
        }

        public virtual int ExecuteSqlCommand(string sql, DynamicParameters param)
        {
            return Connection.Execute(sql, param);
        }

        #region 获取guid

        public string GetUniId()
        {
            return GetUniId("");
        }

        public string GetUniId(string tableName)
        {

            return "";
        }
        public string AddError(string module, string mesg, string strack)
        {

            return "";
        }
        #endregion
        private DateTime fNow;
        #region 获取事务时间
        public DateTime Now
        {
            get
            {
                if (fNow.Value<DateTime>() == default(DateTime))
                {
                    fNow = DateTime.Now;
                }
                return fNow;
            }
        }
        #endregion

        #region 事务注册
        public List<DbTrans> SqlTransList = new List<DbTrans>();
        public List<IUnitOfDapper> DbContextList = new List<IUnitOfDapper>();

        public int RegisterSqlCommand(string sql,  DynamicParameters param)
        {
            DbTrans trans = new DbTrans()
            {
                Sql = sql,
                Param = param,
                CommandType = CommandType.Text
            };
            SqlTransList.Add(trans);
            return 1;
        }
        #endregion

        public string GetSqlParamName(string param)
        {
            return "@" + param;
        }

        public int Submit()
        {
            CheckSubmit();
            return ADOSubmit();
        }
        private bool fIsSubmit;
        private void CheckSubmit()
        {
            if (!fIsSubmit)
            {
                fIsSubmit = true;
            }
            else
            {
                throw new Exception("注意 DbContext 只能被提交一次");
            }
        }

        public List<Func<IDbTransaction, int>> ApplyFun
        {
            get;
            set;
        }

        public StringBuilder LogCommand(StringBuilder sb)
        {
            //sb.AppendLine("_一个子工作单元");
            //bool _islog = LogSaveAtawChanges();
            //if (_islog || !IsChildUnit)
            //{
            //    if ((CommandItems != null && CommandItems.Count > 0))
            //    {
            //        CommandItems.CommandToString(sb);
            //    }
            //    if (SqlTransList != null && SqlTransList.Count > 0)
            //    {
            //        SqlTransList.SqlTransToString(sb);
            //    }
            //}

            return sb;
        }

        // public void 
        private bool IsSaveAtawChanges = false;

        private bool LogSaveAtawChanges()
        {
            //try
            //{
            //    if (!IsSaveAtawChanges)
            //    {
            //        IsSaveAtawChanges = true;
            //        SaveAtawChanges();
            //    }
            //    else
            //        return false;//已经被记录过了
            //}
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //{
            //    //string exx = string.Join("|",
            //    //    dbEx.EntityValidationErrors.Select(
            //    //    a => string.Join("-", a.ValidationErrors.Select(b => b.ErrorMessage),"-实体:",a.Entry.Entity.ToString(),"-状态",a.Entry.State.ToString())
            //    //    ),Environment.NewLine);
            //    //exx = exx + AtawAppContext.Current.FastJson.ToJSON(dbEx.EntityValidationErrors);
            //    string exx = this.logDbEx(dbEx);
            //    AtawTrace.WriteFile(LogType.EfErrot, exx);
            //    throw dbEx;
            //}

            return true;
        }




        public bool IsChildUnit
        {
            get;
            set;
        }

        public int ADOSubmit()
        {

            bool _haslog = LogSaveAtawChanges();//是否需要记录日志

            var con = Connection;
            //check
            int result = 0;

            if ((SqlTransList.IsNotEmpty()) || DbContextList.IsNotEmpty())
            {
                using (con)
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var trans = con.BeginTransaction();

                    using (trans)
                    {
                        try
                        {

                            if (SqlTransList.Count > 0)
                            {
                                foreach (var sql in SqlTransList)
                                {
                                    result += con.Execute(sql.Sql, sql.Param, trans, null, sql.CommandType);
                                }
                            }

                            foreach (var context in DbContextList)
                            {
                                result = result + context.Submit();
                            }
                            if (ApplyFun != null && ApplyFun.Count > 0)
                            {
                                foreach (var fun in ApplyFun)
                                {
                                    result = result + fun(null);
                                }
                            }
                            //if (!IsChildUnit)
                            //{
                            //    AtawTrace.WriteFile(LogType.SubmitSql, sb.ToString());
                            //}
                            trans.Commit();

                        }
                        //catch (System.Data.Common.DbException wex)
                        //{
                        //    trans.Rollback();
                        //    throw wex;
                        //}

                        catch (Exception ex)
                        {

                            trans.Rollback();
                            throw ex;

                        }
                        finally
                        {

                            con.Close();
                        }

                        //catch (SqlException sqlEx)
                        //{
                        //    trans.Rollback();
                        //    throw sqlEx;
                        //}
                    }
                }
            }

            return result;
        }



        #region 存储过程

        public virtual int ExecuteStored(string storedName,  DynamicParameters param)
        {
            return ExecuteSqlCommand(" EXECUTE " + storedName, param);
        }
        public int RegisterStored(string storedName,  DynamicParameters param)
        {
            DbTrans trans = new DbTrans()
            {
                Sql = storedName,
                Param = param,
                CommandType = CommandType.StoredProcedure
            };
            SqlTransList.Add(trans);
            return 1;
        }
        #endregion
        public T QueryObject<T>(string sqlString,  DynamicParameters cmdParms)

        {
            var conn = Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }



            var res = conn.QueryFirst<T>(sqlString, cmdParms);
            if (conn != null)
                conn.Close();
            return res;
        }
        public object QueryObject(string sqlString,  DynamicParameters cmdParms)

        {
            var conn = Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }



            var res = conn.QueryFirst(sqlString, cmdParms);
            if (conn != null)
                conn.Close();
            return res;
        }
        public DataSet QueryDataSet(string sqlString, DynamicParameters cmdParms)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            var conn = Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var reader = conn.ExecuteReader(sqlString, cmdParms);
            dt.Load(reader);
            ds.Tables.Add(dt);
            if (conn != null)
                conn.Close();
            return ds;


        }
        public DataSet QueryDataSet(string sqlString)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            var conn = Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var reader = conn.ExecuteReader(sqlString);
            dt.Load(reader);
            ds.Tables.Add(dt);
            if (conn != null)
                conn.Close();
            return ds;
        }


        public void AddUnitOfData(IUnitOfDapper unitOfData)
        {
            //throw new NotImplementedException();
            this.DbContextList.Add(unitOfData);
        }

        private string fUnitSign;

        public string UnitSign
        {
            get
            {
                return fUnitSign;
            }
        }

        
       

       


        



        public List<IUnitOfDapper> UnitOfDataList
        {
            get
            {
                return this.DbContextList;
            }
            set
            {
                this.DbContextList = value;
            }
        }
    }
}
