using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core.Model
{
    public class FormSummaryInfo
    {
        List<FormInfo> formInfos = new List<FormInfo>();

        public List<FormInfo> FormInfos { get => formInfos; set => formInfos = value; }
    }

    public class FormInfo
    {
        public string FormName { get; set; }

        public string FormTableName { get; set; }

        public string FormEnName { get; set; }

        public string FormCnName { get; set; }
    }
}
