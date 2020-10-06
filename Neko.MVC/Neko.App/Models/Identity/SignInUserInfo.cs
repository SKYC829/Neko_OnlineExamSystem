using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Identity
{
    [Serializable]
    public class SignInUserInfo
    {
        [Required(ErrorMessage = "{0}不允许为空！"),Display(Name = "帐号:",Prompt ="学号、用户名")]
        public string CreditId { get; set; }

        [Required(ErrorMessage = "{0}不允许为空！"),Display(Name = "密码:", Prompt = "密码"),DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Display(Name = "记住我",Prompt = "记住我")]
        public bool RememberMe { get; set; }
    }
}
