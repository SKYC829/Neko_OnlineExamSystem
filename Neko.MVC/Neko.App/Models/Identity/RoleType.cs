using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Identity
{
    [Flags]
    public enum RoleType
    {
        [Display(Name = "超级管理员")]
        Admin = 0,
        [Display(Name = "教师")]
        Teacher = 2,
        [Display(Name = "学生")]
        Student = 4,
    }
}
