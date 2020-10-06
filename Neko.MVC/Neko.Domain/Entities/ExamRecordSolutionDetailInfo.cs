using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_ExamRecordSolutionDetail")]
    public class ExamRecordSolutionDetailInfo:DbEntity
    {
        public int DetailId { get; set; }

        public int SolutionId { get; set; }

        public bool IsCorrect { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual UserInfo CreateUser { get; set; }

        public virtual SolutionInfo Solution { get; set; }

        public virtual ExamRecordQuestionDetailInfo QuestionRecord { get; set; }
    }
}
