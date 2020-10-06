using Neko.App.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class ExamRecordInfo
    {
        public int Id { get; set; }

        public int ExamPaperId { get; set; }

        [Display(Name = "考试分数")]
        public double ExamScore { get; set; }

        [Display(Name = "是否及格")]
        public bool IsPassed { get; set; }

        public string LeftTime { get; set; }

        public int CreateUserId { get; set; }

        [Display(Name = "答卷人")]
        public UserInfo CreateUser { get; set; }

        public DateTime BeginTime { get; set; }

        [Display(Name = "考试时间")]
        public DateTime CreateDate { get; set; }

        public ExamPaperInfo ExamPaper { get; set; }

        public ICollection<ExamQuestionRecordInfo> QuestionRecords { get; set; }
    }
}
