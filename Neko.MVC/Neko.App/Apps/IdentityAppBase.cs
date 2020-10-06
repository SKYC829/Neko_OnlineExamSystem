using Neko.App.Models.Identity;
using Neko.Unity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Apps
{
    public abstract class IdentityAppBase
    {
        protected UserInfo _loginUser { get; set; }

        public IdentityAppBase()
        {
            GlobalConfig.CacheCookies.TryGetValue("Neko_Asp_User", out object user);
            UserInfo usr = user as UserInfo;
            if (usr != null)
            {
                //throw new Exception("请重新登录系统");
                _loginUser = usr;
            }
            else
            {
                _loginUser = new UserInfo();
            }
        }
    }
}
