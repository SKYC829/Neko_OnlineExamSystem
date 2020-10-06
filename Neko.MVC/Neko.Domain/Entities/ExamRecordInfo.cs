using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_ExamRecord")]
    public class ExamRecordInfo:DbEntity
    {
        public int ExamPaperId { get; set; }

        public double ExamScore { get; set; }

        public bool IsPassed { get; set; }

        public string LeftTime { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime BeginTime { get; set; }

        public virtual UserInfo CreateUser { get; set; }

        public virtual ExamPaperInfo ExamPaper { get; set; }

        public virtual ICollection<ExamRecordQuestionDetailInfo> QuestionRecords { get; set; }
    }
}
