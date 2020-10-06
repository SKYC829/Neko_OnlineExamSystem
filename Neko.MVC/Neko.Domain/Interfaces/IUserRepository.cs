using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Domain.Interfaces
{
    public interface IUserRepository:IRepository<UserInfo>
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="creditId">帐号、用户名</param>
        /// <param name="passwordHash">密码的哈希值</param>
        /// <returns></returns>
        UserInfo SignIn(string creditId, string passwordHash);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        UserInfo SignUp(string workId, string userName, string password,int roleId, string remark = "");

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="creditId"></param>
        /// <returns></returns>
        bool SignOut(string creditId);

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool IsExists(Expression<Func<UserInfo, bool>> expression);
    }
}
