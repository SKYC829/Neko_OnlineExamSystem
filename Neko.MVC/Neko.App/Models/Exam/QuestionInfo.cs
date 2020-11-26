using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class QuestionInfo
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}不允许为空"),Display(Name = "问题",Prompt = "题目内容")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请选择一个{0}"),Display(Name = "题目类型",Prompt = "题目类型")]
        public QuestionType QuestionType { get; set; }

        [Required(ErrorMessage = "请填写一个{0}"),Display(Name = "题目分值",Prompt = "该题总分为多少分")]
        public double QuestionScore { get; set; }

        public string QuestionGroupName { get; set; }

        public IEnumerable<SolutionInfo> Solutions { get; set; }
    }
}
