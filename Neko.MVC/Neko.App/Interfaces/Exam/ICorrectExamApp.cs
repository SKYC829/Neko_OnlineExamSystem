using Neko.App.Models.Exam;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neko.App.Interfaces.Exam
{
    public interface ICorrectExamApp
    {
        Task<ExamRecordInfo> CorrectExam(ExamSubmitInfo submitInfo);
    }
}
