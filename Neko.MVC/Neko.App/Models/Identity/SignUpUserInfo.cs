using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Identity
{
    [Serializable]
    public class SignUpUserInfo
    {
        [Display(Name = "学号",Prompt = "学生的学号或教师的工号"),RegularExpression(@"-?[1-9]\d*",ErrorMessage = "学号或工号只允许是数字")]
        public string WorkId { get; set; }

        [Display(Name = "姓名",Prompt = "你的名字")]
        public string UserName { get; set; }

        [Display(Name = "密码",Prompt = "登录系统的密码"),DataType(DataType.Password), Compare("Password", ErrorMessage = "两次输入的密码不一致！")]
        public string Password { get; set; }

        [Display(Name = "确认密码",Prompt = "把上面的密码再输入一次"), DataType(DataType.Password),Compare("Password",ErrorMessage = "两次输入的密码不一致！")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "角色",Prompt = "选择一个角色给这个用户")]
        public int RoleId { get; set; }

        [Display(Name = "备注",Prompt = "这个用户的备注")]
        public string Remark { get; set; }
    }
}
