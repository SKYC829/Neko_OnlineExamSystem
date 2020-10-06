using Neko.App.Interfaces.Identity;
using Neko.App.Models.Identity;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Util;
using Util.DataObject;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Apps.IdentityApps
{
    public class UserApp :IUserApp
    {
        private readonly IUserRepository _repository;
        private readonly IRoleApp _roleApp;
        public UserApp(IUserRepository repository, IRoleApp roleApp)
        {
            _repository = repository;
            _roleApp = roleApp;
        }

        public UserInfo Load(string creditId)
        {
            DbModel.UserInfo usr = _repository.Load(p => p.WorkId.Equals(creditId) || p.Id.ToString().Equals(creditId) || p.UserName.Equals(creditId));
            if (usr == null)
            {
                return null;
            }
            UserInfo user = new UserInfo();
            user.Id = usr.Id;
            user.WorkId = usr.WorkId;
            user.UserName = usr.UserName;
            user.Password = usr.Password;
            user.RoleId = usr.RoleId;
            user.Role = _roleApp.Load(user.RoleId);
            user.Remark = usr.Remark;
            return user;
        }

        public UserInfo SignIn(SignInUserInfo userInfo)
        {
            DbModel.UserInfo usr = _repository.SignIn(userInfo.CreditId, userInfo.PasswordHash.EncryptMD5());
            if(usr == null)
            {
                return null;
            }
            UserInfo user = new UserInfo();
            user.Id = usr.Id;
            user.WorkId = usr.WorkId;
            user.UserName = usr.UserName;
            user.Password = usr.Password;
            user.RoleId = usr.RoleId;
            user.Role = _roleApp.Load(user.RoleId);
            user.Remark = usr.Remark;
            return user;
        }

        public bool SignOut(string creditId)
        {
            return _repository.SignOut(creditId);
        }

        public UserInfo SignUp(SignUpUserInfo userInfo)
        {
            DbModel.UserInfo usr = _repository.SignUp(userInfo.WorkId, userInfo.UserName, userInfo.Password, userInfo.RoleId, userInfo.Remark);
            if (usr == null)
            {
                return null;
            }
            UserInfo user = new UserInfo();
            user.Id = usr.Id;
            user.WorkId = usr.WorkId;
            user.UserName = usr.UserName;
            user.Password = usr.Password;
            user.RoleId = usr.RoleId;
            user.Role = _roleApp.Load(user.RoleId);
            user.Remark = usr.Remark;
            return user;
        }
        public IQueryable<UserInfo> Query()
        {
            IQueryable<DbModel.UserInfo> usrs = _repository.All().AsQueryable();
            List<UserInfo> usr = new List<UserInfo>();
            foreach (DbModel.UserInfo user in usrs)
            {
                UserInfo info = new UserInfo
                {
                    Id = user.Id,
                    WorkId = user.WorkId,
                    UserName = user.UserName,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    Remark = user.Remark
                };
                usr.Add(info);
            }
            foreach (UserInfo item in usr)
            {
                item.Role = _roleApp.Load(item.RoleId);
            }
            return usr.AsQueryable<UserInfo>();
        }
        public IQueryable<UserInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<DbModel.UserInfo, bool>> whereBy, Expression<Func<DbModel.UserInfo, int>> orderBy)
        {
            IQueryable<DbModel.UserInfo> usrs = _repository.Query(pageIndex, out totalCount, out totalPage,whereBy, orderBy);
            List<UserInfo> usr = new List<UserInfo>();
            foreach (DbModel.UserInfo user in usrs)
            {
                UserInfo info = new UserInfo
                {
                    WorkId = user.WorkId,
                    UserName = user.UserName,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    Remark = user.Remark
                };
                usr.Add(info);
            }
            foreach (UserInfo item in usr)
            {
                item.Role = _roleApp.Load(item.RoleId);
            }
            return usr.AsQueryable<UserInfo>();
        }

        public void Remove(int userId)
        {
            _repository.Remove(userId);
        }

        public void Remove(UserInfo userInfo)
        {
            DbModel.UserInfo usr = _repository.Load(p => p.WorkId.Equals(userInfo.WorkId));
            _repository.Remove(usr);
        }
    }
}
