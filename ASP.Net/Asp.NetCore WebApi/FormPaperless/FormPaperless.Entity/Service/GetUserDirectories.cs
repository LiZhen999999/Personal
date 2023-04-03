using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    public class GetUserDirectories : ServiceBase
    {
        SqlSugarClient sqlCilent;
        SqlSugarClient sqlReadOnlyCilent;

        public GetUserDirectories(ServerInfo serverInfo) : base(serverInfo)
        {
            sqlCilent = SqlSugarExtension.GetPtnCilent(serverInfo.MesSrv, serverInfo.MesDb);
            sqlReadOnlyCilent = SqlSugarExtension.GetPtnReadOnlyCilent(serverInfo.MesSrv, serverInfo.MesDb);
        }

        public string GetDirectories(int userId)
        {
            var result = sqlReadOnlyCilent.Queryable<tbSheetPaperlessDirectories, tbSheetPaperlessDirectoryPermissions, tbSheetPaperlessUsers>((d, p, u) =>
                new JoinQueryInfos(
                    JoinType.Left, d.DirectoryID == p.DirectoryID,
                    JoinType.Left, p.UserID == u.UserID
                ))
                .Where((d, p, u) => u.UserID == userId)
                .Select((d, p, u) => new
                {
                    DirectoryId = d.DirectoryID,
                    DirectoryName = d.DirectoryName,
                    CanRead = p.CanView,
                    CanWrite = p.CanEdit
                })
                .ToList();
            string json = JsonSerializer.Serialize(result);
            return json;
        }
    }
}
