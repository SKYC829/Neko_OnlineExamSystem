using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_ExamRecordQuestionDetail")]
    public class ExamRecordQuestionDetailInfo:DbEntity
    {
        public int RecordId { get; set; }

        public int QuestionId { get; set; }

        public double QuestionScore { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual UserInfo CreateUser { get; set; }

        public virtual ExamRecordInfo ExamRecord { get; set; }

        public virtual QuestionInfo Question { get; set; }

        public virtual ICollection<ExamRecordSolutionDetailInfo> SolutionRecords { get; set; }
    }
}
