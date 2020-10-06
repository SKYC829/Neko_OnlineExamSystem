using Neko.App.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Interfaces.Identity
{
    public interface IUserApp
    {
        UserInfo SignIn(SignInUserInfo userInfo);

        UserInfo SignUp(SignUpUserInfo userInfo);

        bool SignOut(string creditId);

        UserInfo Load(string creditId);

        IQueryable<UserInfo> Query();

        IQueryable<UserInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<DbModel.UserInfo, bool>> whereBy, Expression<Func<DbModel.UserInfo, int>> orderBy);

        void Remove(int userId);

        void Remove(UserInfo userInfo);
    }
}
