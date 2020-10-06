using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using Neko.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Util;
using App = Neko.App.Models;

namespace Neko.Data.Repositories
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(NekoDbContext context) : base(context)
        {
        }

        public bool IsExists(Expression<Func<UserInfo, bool>> expression)
        {
            return Load(expression) != null;
        }

        public IQueryable<UserInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<UserInfo, bool>> whereBy, Expression<Func<UserInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }

        public UserInfo SignIn(string creditId, string passwordHash)
        {
            return Load(p => (p.WorkId.Equals(creditId) || p.UserName.Equals(creditId)) && p.Password_Hash.Equals(passwordHash));
        }

        public bool SignOut(string creditId)
        {
            return true;
        }

        public UserInfo SignUp(string workId, string userName, string password, int roleId, string remark = "")
        {
            GlobalConfig.CacheCookies.TryGetValue("Neko_Asp_User", out object currentUsr);
            UserInfo usr = new UserInfo();
            usr.WorkId = workId;
            usr.UserName = userName;
            usr.Password = password;
            usr.Password_Hash = usr.Password.EncryptMD5();
            usr.Remark = remark;
            usr.RoleId = roleId;
            var user = Load(p => p.UserName.Equals((currentUsr as App.Models.Identity.UserInfo).UserName));
            usr.CreateUserId = user.Id;
            usr.CreateDate = DateTime.Now;
            usr = Save(usr);
            return usr;
        }
    }
}
