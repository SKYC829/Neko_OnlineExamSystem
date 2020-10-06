using Neko.App.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Interfaces.Identity
{
    public interface IRoleApp
    {
        RoleInfo Load(int roleId);

        IQueryable<RoleInfo> Query(int pageIndex, out int totalCount,out int totalPage ,Expression<Func<DbModel.RoleInfo, bool>> whereBy, Expression<Func<DbModel.RoleInfo, int>> orderBy);

        IQueryable<RoleInfo> Query();

        ICollection<MenuInfo> GetRoleMenu(int roleId);

        ICollection<MenuInfo> GetRoleMenu(Expression<Func<DbModel.RoleMenu, bool>> expression);
    }
}
