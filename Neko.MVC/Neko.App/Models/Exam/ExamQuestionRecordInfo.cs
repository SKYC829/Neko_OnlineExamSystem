using Neko.App.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class ExamQuestionRecordInfo
    {
        public int Id { get; set; }

        public int RecordId { get; set; }

        public int QuestionId { get; set; }

        public double QuestionScore { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public UserInfo CreateUser { get; set; }

        public ExamRecordInfo ExamRecord { get; set; }

        public QuestionInfo Question { get; set; }

        public ICollection<ExamSolutionRecordInfo> SolutionRecords { get; set; }
    }
}
