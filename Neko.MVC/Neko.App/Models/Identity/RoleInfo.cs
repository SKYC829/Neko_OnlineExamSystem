using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Identity
{
    [Serializable]
    public class RoleInfo
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "{0}不允许为空！"),Display(Name = "角色名称:",Prompt = "角色名称")]
        public string RoleName { get; set; }

        [Display(Name = "角色类型:")]
        public RoleType RoleType { get; set; }

        [Display(Name = "备注:")]
        public string Remark { get; set; }

        public IEnumerable<MenuInfo> RoleMenus { get; set; }
    }
}
