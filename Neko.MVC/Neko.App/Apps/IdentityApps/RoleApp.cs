using Neko.App.Interfaces.Identity;
using Neko.App.Models.Identity;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Apps.IdentityApps
{
    public class RoleApp :IRoleApp
    {
        private readonly IRoleRepository _respository;
        private readonly IMenuRepository _menuRepository;
        public RoleApp(IRoleRepository respository,IMenuRepository menuRepository)
        {
            _respository = respository;
            _menuRepository = menuRepository;
        }

        public ICollection<MenuInfo> GetRoleMenu(int roleId)
        {
            return GetRoleMenu(p => p.RoleId.Equals(roleId));
        }

        public ICollection<MenuInfo> GetRoleMenu(Expression<Func<DbModel.RoleMenu, bool>> expression)
        {
            ICollection<DbModel.MenuInfo> roleMenus = _respository.GetRoleMenu(expression);
            List<MenuInfo> menus = new List<MenuInfo>();
            foreach (DbModel.MenuInfo roleMenu in roleMenus)
            {
                MenuInfo menu = new MenuInfo();
                menu.MenuIndex = roleMenu.MenuIndex;
                menu.MenuName = roleMenu.MenuName;
                menu.Area = roleMenu.Area;
                menu.Controller = roleMenu.Controller;
                menu.Action = roleMenu.Action;
                menu.SubMenus = new List<MenuInfo>();
                foreach (DbModel.MenuInfo subMenu in _menuRepository.LoadSubMenus(roleMenu.Id))
                {
                    MenuInfo sub = new MenuInfo();
                    sub.MenuIndex = subMenu.MenuIndex;
                    sub.MenuName = subMenu.MenuName;
                    sub.Area = subMenu.Area;
                    sub.Controller = subMenu.Controller;
                    sub.Action = subMenu.Action;
                    menu.SubMenus.Add(sub);
                }
                menus.Add(menu);
            }
            return menus;
        }

        public RoleInfo Load(int roleId)
        {
            DbModel.RoleInfo role = _respository.Load(roleId);
            if(role == null)
            {
                return null;
            }
            RoleInfo result = new RoleInfo();
            result.RoleId = role.Id;
            result.RoleName = role.RoleName;
            result.RoleType = (RoleType)role.RoleType;
            result.Remark = role.Remark;
            result.RoleMenus = GetRoleMenu(role.Id);
            return result;
        }

        public IQueryable<RoleInfo> Query(int pageIndex, out int totalCount,out int totalPage, Expression<Func<DbModel.RoleInfo, bool>> whereBy, Expression<Func<DbModel.RoleInfo, int>> orderBy)
        {
            IQueryable<DbModel.RoleInfo> roles = _respository.Query(pageIndex, out totalCount, out totalPage,whereBy, orderBy);
            List<RoleInfo> results = new List<RoleInfo>();
            foreach (DbModel.RoleInfo item in roles)
            {
                RoleInfo result = new RoleInfo();
                result.RoleId = item.Id;
                result.RoleName = item.RoleName;
                result.RoleType = (RoleType)item.RoleType;
                result.Remark = item.Remark;
                result.RoleMenus = GetRoleMenu(item.Id);
                results.Add(result);
            }
            return results.AsQueryable<RoleInfo>();
        }

        public IQueryable<RoleInfo> Query()
        {
            IQueryable<DbModel.RoleInfo> roles = _respository.All().AsQueryable();
            List<RoleInfo> results = new List<RoleInfo>();
            foreach (DbModel.RoleInfo item in roles)
            {
                RoleInfo result = new RoleInfo();
                result.RoleId = item.Id;
                result.RoleName = item.RoleName;
                result.RoleType = (RoleType)item.RoleType;
                result.Remark = item.Remark;
                result.RoleMenus = GetRoleMenu(item.Id);
                results.Add(result);
            }
            return results.AsQueryable<RoleInfo>();
        }
    }
}
