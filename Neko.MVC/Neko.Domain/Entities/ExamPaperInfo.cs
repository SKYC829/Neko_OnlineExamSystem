using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_ExamPapers")]
    public class ExamPaperInfo:DbEntity
    {
        public string ExamName { get; set; }

        public int ExamMinute { get; set; }

        public double ExamScore { get; set; }

        public int QuestionNums { get; set; }

        public DateTime ExamDateFrom { get; set; }

        public DateTime ExamDateTo { get; set; }

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

        public virtual ICollection<ExamQuestionInfo> Questions { get; set; }


    }
}
