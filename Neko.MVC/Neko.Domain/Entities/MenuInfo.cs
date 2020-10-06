using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("Tb_Menu")]
    public class MenuInfo:DbEntity
    {
        /// <summary>
        /// 父级菜单编号
        /// </summary>
        public int ParentId { get; set; }

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

        /// <summary>
        /// 菜单备注
        /// </summary>
        public string Remark { get; set; }
    }
}
