using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Domain.Interfaces
{
    public interface IRoleRepository : IRepository<RoleInfo>
    {

        ICollection<MenuInfo> GetRoleMenu(int roleId);

        ICollection<MenuInfo> GetRoleMenu(Expression<Func<RoleMenu, bool>> expression);
    }
}
