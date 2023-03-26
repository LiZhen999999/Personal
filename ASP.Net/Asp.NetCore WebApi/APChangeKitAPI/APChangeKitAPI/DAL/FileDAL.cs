using APChangeKitAPI.Common;
using APChangeKitAPI.Models.Entity;
using DbUtils;
using System.Collections.Generic;
using System;

namespace APChangeKitAPI.DAL
{
    public class FileDAL
    {
        private readonly IDbUtils _dbUtils = new DbFactory(DatabaseType.SqlServer, AppSettings.Data.ConnectionStrings.EMSDB).CreateDbUtils();

        public IEnumerable<T_File> GetFile(T_File param)
        {
            if (param == null)
                throw new ArgumentNullException("参数不能为NULL！");

            var sql = "SELECT * FROM dbo.T_File WHERE 1 = 1 ";
            if (param.ID > 0)
                sql += " AND ID = @ID ";

            if (!string.IsNullOrEmpty(param.TrueFileName))
                sql += " AND TrueFileName = @TrueFileName ";

            return _dbUtils.Query<T_File>(sql, param);
        }

        public int InsertFile(T_File param)
        {
            if (param == null)
                throw new ArgumentNullException("参数不能为NULL！");

            var sql = @"
INSERT INTO dbo.T_File(TrueFileName, FileName, Extension, FilePath, CreateBy, UpdateBy)
VALUES(@TrueFileName,
@FileName, -- FileName - nvarchar(200)
@Extension  , -- Extension - varchar(20)
@FilePath , -- FilePath - nvarchar(2000)
@CreateBy, -- CreateBy - nvarchar(50)
@UpdateBy -- UpdateBy - nvarchar(50)
);SELECT @@IDENTITY;";

            return _dbUtils.ExecuteScalar<int>(sql, param);
        }
    }
}
