using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    public class tbSheetPaperlessDirectories
    {
        public int DirectoryID { get; set; }
        public string DirectoryName { get; set; }
        public int? ParentDirectoryID { get; set; }
    }
}
