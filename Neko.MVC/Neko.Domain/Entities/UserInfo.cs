using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("Tb_User")]
    public class UserInfo:DbEntity
    {
        /// <summary>
        /// 学号、工号
        /// </summary>
        public string WorkId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 密码（已加密）
        /// </summary>
        public string Password_Hash { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsRemove { get; set; }

        /// <summary>
        /// 是否已锁定
        /// </summary>
        public bool IsLock { get; set; }

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
        /// 角色实体
        /// </summary>
        public virtual RoleInfo Role { get; set; }

        public virtual ICollection<ExamRecordInfo> ExamRecords { get; set; }
    }
}
