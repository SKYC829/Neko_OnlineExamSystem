using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class ExamPaperInfo:EditExamPaperInfo
    {

        [Display(Name = "包含题目数量")]
        public int QuestionsNum { get; set; }
    }
}
