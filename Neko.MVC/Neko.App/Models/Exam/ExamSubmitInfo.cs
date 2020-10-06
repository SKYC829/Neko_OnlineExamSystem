using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class ExamSubmitInfo
    {
        public int examId { get; set; }
        public string leftTime { get; set; }
        public List<ExamDetailSubmitInfo> details { get; set; }
    }
}
