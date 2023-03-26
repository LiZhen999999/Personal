using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Models
{
    public class AP_ChangeKit_Control
    {
        public int ID { get; set; }
        public string ControlExpression { get; set; }
        public string ControlTip { get; set; }
        public string InputType { get; set; }
        public string ValueType { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
