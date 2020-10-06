using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class CreateQuestionInfo
    {
        [Required(ErrorMessage = "{0}不允许为空"), Display(Name = "问题", Prompt = "题目内容")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请选择一个{0}"), Display(Name = "题目类型", Prompt = "题目类型")]
        public QuestionType QuestionType { get; set; }

        public int QuestionTypeInt { get; set; }

        [Required(ErrorMessage = "请填写一个{0}"), Display(Name = "题目分值", Prompt = "该题总分为多少分"),Compare("TotalSolutionScore",ErrorMessage = "题目分值不符！请检查答案分值！")]
        public double QuestionScore { get; set; }

        public IEnumerable<string> SolutionNames { get; set; }

        public IEnumerable<double> SolutionScore { get; set; }

        public double TotalSolutionScore
        {
            get
            {
                double result = 0;
                if(SolutionScore != null)
                {
                    foreach (double score in SolutionScore)
                    {
                        result += score;
                    }
                }
                return result;
            }
        }
    }
}
