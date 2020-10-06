using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Neko.App.Models.Identity;
using Neko.MVC.Areas.Identity.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util;
using Util.DataObject;

namespace Neko.MVC.Areas
{
    public abstract class NekoControllerBase:Controller
    {
        protected UserInfo loginUser;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取Session里当前登录的用户
            UserInfo usr = context.HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if(usr == null)
            {
                //如果没登录并且不是账户验证用的控制器的话，跳转到登录页面
                if (context.Controller.GetType() != typeof(AccountController))
                {
                    context.Result = new RedirectResult("/Account/SignIn");
                }
                else
                {
                    string action = ObjectUtil.Get<string>(context.ActionDescriptor.RouteValues, "action");
                    switch (action)
                    {
                        case "SignIn":
                        case "SignOut":
                            break;
                        default:
                            context.Result = new RedirectResult("/Account/SignIn");
                            break;
                    }
                }
            }
            else
            {
                //否则把已登录用户存起来给前台获取
                ViewBag.Neko_Asp_User = usr;
                loginUser = usr;
            }
            base.OnActionExecuting(context);
        }
    }
}
