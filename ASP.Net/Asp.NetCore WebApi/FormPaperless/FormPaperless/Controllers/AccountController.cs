using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.DirectoryServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using FormPaperless.Core;
using AutoMapper;

namespace FormPaperless.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        IConfiguration Configuration;
        ServerInfo ServerInfo;


        public AccountController(IConfiguration configuration, ServerInfo serverInfo)
        {
            Configuration = configuration;
            ServerInfo = serverInfo;
        }

        /// <summary>
        /// 用户账号登录
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseObjectModel<UserInformation> LoginAccount([FromBody] LoginInfo loginInfo)
        {

            // 用户信息实体
            UserInformation user = new UserInformation();

            // 获取当前用户的ClaimsIdentity对象
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            // 获取用户名对应的Claim对象
            var nameClaim = identity.FindFirst(ClaimTypes.Name);

            if (nameClaim != null)
            {
                // 获取用户名
                string UName = HttpContext.User.FindFirst("userName")?.Value;

                if (UName.ToUpper() == loginInfo.UserName.ToUpper())
                {
                    user.UserName = UName;
                    user.Department = HttpContext.User.FindFirst("Department")?.Value;
                    user.Account = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                    return user;
                }
            }

            // 集成域判断
            var domainAndName = HttpContext.User.Identity.Name;

            // 域验证的域名
            string strDomainName = Configuration["DomainName"];

            // 利用AD验证帐号密码是否正确
            string myLoginUser = loginInfo.UserName;
            string password = loginInfo.Password;
            DirectoryEntry userEntry = new System.DirectoryServices.DirectoryEntry("LDAP://" + strDomainName, myLoginUser, password);
            DirectorySearcher mySearcher = new DirectorySearcher(userEntry);
            mySearcher.Filter = "sAMAccountname=" + myLoginUser;
            mySearcher.SearchScope = SearchScope.Subtree;

            // 获取Session中的信息
            var info = HttpContext.Session.GetString("UserId");

            bool b_LoginSuccess = false;
            try
            {
                SearchResultCollection results = mySearcher.FindAll();
                if (results.Count > 0)
                {
                    // 获取域账户中的工号信息
                    ResultPropertyCollection myResultPropColl = results[0].Properties;
                    // physicaldeliveryofficename[工号的字段名]
                    string userId = myResultPropColl["physicaldeliveryofficename"][0].ToString();
                    string userName = myResultPropColl["name"][0].ToString();
                    string userDepartment = myResultPropColl["department"][0].ToString();


                    b_LoginSuccess = true;

                    // 如果登录成功，则把用户信息写入到Cookie
                    var claims = new List<Claim>() //用Claim保存用户信息
                    {
                        new Claim(ClaimTypes.Name, userId),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim("Department", userDepartment),
                        new Claim("userName", userName),
                        new Claim("password", password)
                    };

                    // 把用户信息装到ClaimsPrincipal
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));

                    // 登录，把用户信息写入到cookie
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.Now.AddMinutes(60) // 过期时间60分钟
                        }).Wait();

                    user.Account = userId;
                    user.UserName = userName;
                    user.Department = userDepartment;
                }
                else
                {
                    b_LoginSuccess = false;
                }
            }
            catch(Exception err)
            {
                b_LoginSuccess = false;
            }

            if (b_LoginSuccess)
            {
                return user;
            }
            else
            {
                return FailResult<UserInformation>("登录失败");
            }
        }


        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseObjectModel<string> LogoutAccount()
        {
            bool b_Logout = false;
            try
            {
                //移除当前Session中登录的用户信息
                //HttpContext.Session.Remove("UserId");
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                b_Logout = true;
            }
            catch
            {
                b_Logout = false;
            }
            if (b_Logout)
            {
                return SuccessResult<string>("登出成功！");
            }
            else
            {
                return FailResult<string>("登出失败！");
            }
        }
    }
}
