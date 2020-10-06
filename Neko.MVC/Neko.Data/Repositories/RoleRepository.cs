using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class RoleRepository : BaseRepository<RoleInfo>, IRoleRepository
    {
        public RoleRepository(NekoDbContext context) : base(context)
        {
        }

        public ICollection<MenuInfo> GetRoleMenu(int roleId)
        {
            return GetRoleMenu(p => p.RoleId.Equals(roleId));
        }

        public ICollection<MenuInfo> GetRoleMenu(Expression<Func<RoleMenu,bool>> expression)
        {
            var roleMenus = _dbContext.Set<RoleMenu>().Where(expression);
            var menus = from t in roleMenus select t.Menu;
            return menus.ToList();
        }

        public IQueryable<RoleInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<RoleInfo, bool>> whereBy, Expression<Func<RoleInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
