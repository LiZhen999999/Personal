using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    //定义登录时传送的数据信息
    public class LoginInfo
    {
        private string userName;
        private string password;

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
    }
}
