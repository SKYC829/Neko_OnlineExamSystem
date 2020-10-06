using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Domain.Interfaces
{
    public interface IMenuRepository:IRepository<MenuInfo>
    {
        IEnumerable<MenuInfo> LoadSubMenus(int menuParentId);

        IEnumerable<MenuInfo> LoadSubMenus(Expression<Func<MenuInfo, bool>> expression);
    }
}
