using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_Questions")]
    public class QuestionInfo : DbEntity
    {

        public string QuestionName { get; set; }

        public QuestionType QuestionType { get; set; }

        [Column(name:"Score")]
        public double QuestionScore { get; set; }

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

        public virtual ICollection<QuestionSolutionInfo> Solutions { get; set; }

        public virtual ICollection<ExamQuestionInfo> Exams { get; set; }
    }
}
