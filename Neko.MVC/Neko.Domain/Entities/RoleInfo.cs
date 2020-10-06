using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("Tb_Role")]
    public class RoleInfo:DbEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsRemove { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人实体
        /// </summary>
        public virtual UserInfo CreateUser { get; set; }

        /// <summary>
        /// 拥有的用户
        /// </summary>
        public virtual ICollection<UserInfo> Users { get; set; }

        /// <summary>
        /// 拥有的菜单
        /// </summary>
        public virtual ICollection<RoleMenu> Menus { get; set; }
    }
}
