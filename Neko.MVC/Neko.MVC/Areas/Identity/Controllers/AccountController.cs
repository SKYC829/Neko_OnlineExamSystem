using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Neko.App.Interfaces.Identity;
using Neko.App.Models.Identity;
using Neko.Unity;
using Util;

namespace Neko.MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("Account")]
    public class AccountController : NekoControllerBase
    {
        private readonly IUserApp _userApp;
        private readonly IRoleApp _roleApp;
        public AccountController(IUserApp userApp,IRoleApp roleApp)
        {
            _userApp = userApp;
            _roleApp = roleApp;
        }

        [Route("SignIn")]
        public IActionResult SignIn()
        {
            SignInUserInfo userInfo = HttpContext.Request.Cookies.GetCookie<SignInUserInfo>("Neko_Asp_User");
            if (userInfo != null)
            {
                return SignIn(userInfo);
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("SignIn")]
        public IActionResult SignIn([FromForm] SignInUserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                UserInfo usr = _userApp.SignIn(userInfo);
                if (usr != null)
                {
                    HttpContext.Session.Set("Neko_Asp_User", usr.ToBytes());
                    if (userInfo.RememberMe)
                    {
                        HttpContext.Response.Cookies.AddCookie("Neko_Asp_User", userInfo, DateTime.Now.AddDays(10));
                    }
                    else
                    {
                        HttpContext.Response.Cookies.DeleteCookie("Neko_Asp_User");
                    }
                    if (GlobalConfig.CacheCookies.ContainsKey("Neko_Asp_User"))
                    {
                        GlobalConfig.CacheCookies.Remove("Neko_Asp_User");
                    }
                    GlobalConfig.CacheCookies.Add("Neko_Asp_User", usr);
                    return Redirect("/Exam/Home");
                }
                ModelState.AddModelError("AccOrPwError", "用户名或密码错误！");
            }
            return View(userInfo);
        }

        [Route("SignUp")]
        public IActionResult SignUp()
        {
            IQueryable<RoleInfo> roles = _roleApp.Query();
            //如果登录的不是超管就不允许创建管理员账户
            UserInfo usr = HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if(usr.Role.RoleType != RoleType.Admin)
            {
                roles = roles.Where(p => p.RoleType != RoleType.Admin);
            }
            ViewBag.Roles = new SelectList(roles,"RoleId","RoleName");
            ViewBag.IsEdit = false;
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Route("SignUp")]
        public IActionResult SignUp([FromForm] SignUpUserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                UserInfo usr = _userApp.SignUp(userInfo);
                if(usr != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("DbError", "注册失败！请稍后再试！");
            }
            IQueryable<RoleInfo> roles = _roleApp.Query();
            //如果登录的不是超管就不允许创建管理员账户
            UserInfo currentUsr = HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if (currentUsr.Role.RoleType != RoleType.Admin)
            {
                roles = roles.Where(p => p.RoleType != RoleType.Admin);
            }
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            ViewBag.IsEdit = false;
            return View(userInfo);
        }

        [Route("SignOut")]
        public IActionResult SignOut()
        {
            UserInfo usr = HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if (usr != null)
            {
                HttpContext.Session.Remove("Neko_Asp_User");
                GlobalConfig.CacheCookies.Remove("Neko_Asp_User");
            }
            return RedirectToAction(nameof(SignIn));
        }

        [Route("Index")]
        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<UserInfo> usrs = _userApp.Query(pageIndex, out int total,out int totalPage, p => !p.IsRemove && p.Role.RoleType != (int)RoleType.Admin , null);
            ViewBag.TotalPages = total;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPage = totalPage;
            return View(usrs);
        }

        [Route("Edit")]
        public IActionResult Edit(string Id)
        {
            UserInfo usr = _userApp.Load(Id);
            if(usr == null)
            {
                return NotFound();
            }
            IQueryable<RoleInfo> roles = _roleApp.Query();
            //如果登录的不是超管就不允许创建管理员账户
            UserInfo user = HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if (user.Role.RoleType != RoleType.Admin)
            {
                roles = roles.Where(p => p.RoleType != RoleType.Admin);
            }
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            ViewBag.IsEdit = true;
            return View(usr);
        }

        [HttpPost]
        [Route("Edit"),ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] UserInfo usr)
        {
            if (ModelState.IsValid)
            {
                usr = _userApp.SignUp(usr);
                if (usr != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("DbError", "修改失败！请稍后再试！");
            }
            IQueryable<RoleInfo> roles = _roleApp.Query();
            //如果登录的不是超管就不允许创建管理员账户
            UserInfo user = HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if (user.Role.RoleType != RoleType.Admin)
            {
                roles = roles.Where(p => p.RoleType != RoleType.Admin);
            }
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            ViewBag.IsEdit = true;
            return View(usr);
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete([FromBody]string id)
        {
            UserInfo usr = _userApp.Load(id);
            if(usr == null)
            {
                return NoContent();
            }
            UserInfo currentUsr = HttpContext.Session.Get("Neko_Asp_User").ToObject<UserInfo>();
            if (usr.WorkId.Equals(currentUsr.WorkId) && usr.Role.RoleType != RoleType.Admin)
            {
                return Problem(detail: "不允许当前用户删除自己！", title: "删除失败", statusCode:405);
            }
            _userApp.Remove(usr);
            return RedirectToAction(nameof(Index));
        }
    }
}
