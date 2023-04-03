using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    //登录后用户的信息
    public class UserInformation
    {
        private string userName;

        private string account;

        private string department;

        public string UserName { get => userName; set => userName = value; }
        public string Account { get => account; set => account = value; }
        public string Department { get => department; set => department = value; }
    }
}
