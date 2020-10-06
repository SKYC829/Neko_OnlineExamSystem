using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class MenuRepository : BaseRepository<MenuInfo>,IMenuRepository
    {
        public MenuRepository(NekoDbContext context) : base(context)
        {
        }

        public IEnumerable<MenuInfo> LoadSubMenus(int menuParentId)
        {
            return LoadSubMenus(p => p.ParentId.Equals(menuParentId));
        }

        public IEnumerable<MenuInfo> LoadSubMenus(Expression<Func<MenuInfo, bool>> expression)
        {
            var menus = _dbContext.Set<MenuInfo>().Where(expression);
            return menus;
        }

        public IQueryable<MenuInfo> Query(int pageIndex, out int totalCount, out int totalPage ,Expression<Func<MenuInfo, bool>> whereBy, Expression<Func<MenuInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
