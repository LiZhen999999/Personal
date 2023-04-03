using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar
{
    public static class SqlSugarExtension
    {
        public static SqlSugarClient GetPtnCilent(this SqlSugarClient client, string sServer, string sDatabaseName)
        {
            string FactoryConnectionString = GetConnectionString(sServer, sDatabaseName, false);
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = FactoryConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public static SqlSugarClient GetPtnReadOnlyCilent(this SqlSugarClient client, string sServer, string sDatabaseName)
        {
            string FactoryConnectionString = GetConnectionString(sServer, sDatabaseName, true);
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = FactoryConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public static SqlSugarClient GetPtnCilent(string sServer, string sDatabaseName)
        {
            string FactoryConnectionString = GetConnectionString(sServer, sDatabaseName, false);
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = FactoryConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        public static SqlSugarClient GetPtnReadOnlyCilent(string sServer, string sDatabaseName)
        {
            string FactoryConnectionString = GetConnectionString(sServer, sDatabaseName, true);
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = FactoryConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        private static string GetConnectionString(string insrvName, string indbName, bool readOnly)
        {
            var sqlLink = new PTN_dbLink.sqlLink();
            var conn = readOnly ? sqlLink.GetSqlReadOnlyCn(insrvName, indbName) : sqlLink.getsqlcn(insrvName, indbName);
            return conn.ConnectionString;
        }
    }
}
