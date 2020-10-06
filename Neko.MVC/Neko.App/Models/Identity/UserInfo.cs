using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Util;

namespace Neko.App.Models.Identity
{
    [Serializable]
    public class UserInfo:SignUpUserInfo
    {
        public int Id { get; set; }
        public string PasswordHash { get { return Password.EncryptMD5(); } }

        [Display(Name = "角色")]
        public RoleInfo Role { get; set; }
    }
}
