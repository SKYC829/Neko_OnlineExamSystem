using Neko.App.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class ExamSolutionRecordInfo
    {
        public int Id { get; set; }

        public int SolutionId { get; set; }

        public bool IsCorrect { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public UserInfo CreateUser { get; set; }

        public SolutionInfo Solution { get; set; }

        public ExamQuestionRecordInfo QuestionRecord { get; set; }
    }
}
