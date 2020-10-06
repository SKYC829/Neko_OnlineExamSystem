using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Models.Identity
{
    [Serializable]
    public class MenuInfo
    {
        /// <summary>
        /// 菜单排序索引
        /// </summary>
        public int MenuIndex { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string Action { get; set; }

        public ICollection<MenuInfo> SubMenus { get; set; }
    }
}
