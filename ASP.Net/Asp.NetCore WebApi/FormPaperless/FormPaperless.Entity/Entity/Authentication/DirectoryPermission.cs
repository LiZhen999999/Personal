using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    public class tbSheetPaperlessDirectoryPermissions
    {
        public int PermissionID { get; set; }
        public int UserID { get; set; }
        public int DirectoryID { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        //public string DirectoryName { get; set; }
        //public bool CanView { get; set; }
        //public bool CanEdit { get; set; }
    }
}
