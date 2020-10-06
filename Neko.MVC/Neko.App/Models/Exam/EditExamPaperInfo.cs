using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class EditExamPaperInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}不允许为空！"),Display(Name = "试卷标题")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}不允许小于5分钟"),Display(Name = "答题时间(分钟)"),Range(5,1440)]
        public int ExamMinute { get; set; }

        [Display(Name = "题目分值")]
        public double ExamScore { get; set; }

        [Required,Display(Name = "考题范围",Prompt = "随机抽取的题目数量"),DataType(DataType.Text)]
        public int QuestionRank { get; set; }

        [Required,Display(Name = "开考时间"),DataType(DataType.DateTime)]
        public DateTime ExamDateFrom { get; set; }

        [Required,Display(Name = "结束时间",Prompt = "考试结束的日期"), DataType(DataType.DateTime)]
        public DateTime ExamDateTo { get; set; }

        [Display(Name = "考生须知")]
        public string Remark { get; set; }

        public ICollection<int> QuestionIds { get; set; }

        public ICollection<QuestionInfo> Questions { get; set; }
    }
}
