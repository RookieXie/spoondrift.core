using Dapper;
using Spoondrift.Code.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Util
{
    public static class SqlUtil
    {
        public static DbTrans SqlByNotAnd(string select, string keyColumn, List<string> valList)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder resSb = new StringBuilder(select);
            int i = 0;
            foreach (string val in valList)
            {
                resSb.Append(" AND ");
                resSb.Append(keyColumn);
                resSb.Append("  <> @p");
                resSb.Append(i);
                parameters.Add("@p" + i.ToString(), val);
                i++;
            }
            return new DbTrans()
            {
                Sql = resSb.ToString(),
                CommandType = CommandType.Text,
                Param = parameters
            };

        }
    }
}
