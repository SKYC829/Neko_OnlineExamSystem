using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class ExamDetailSubmitInfo
    {
        public int questionId { get; set; }

        public List<int> solutionId { get; set; }
    }
}
