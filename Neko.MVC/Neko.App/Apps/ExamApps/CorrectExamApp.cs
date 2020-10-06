using Neko.App.Interfaces.Exam;
using Neko.App.Interfaces.Identity;
using Neko.App.Models.Exam;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Util.DataObject;

namespace Neko.App.Apps.ExamApps
{
    public class CorrectExamApp : IdentityAppBase, ICorrectExamApp
    {
        private readonly IExamRecordApp _recordApp;
        private readonly IQuestionApp _questionApp;
        private readonly IExamPaperApp _examPaperApp;
        private readonly IUserApp _userApp;

        public CorrectExamApp(IExamRecordApp recordApp, IQuestionApp questionApp, IExamPaperApp examPaperApp, IUserApp userApp)
        {
            _recordApp = recordApp;
            _questionApp = questionApp;
            _examPaperApp = examPaperApp;
            _userApp = userApp;
        }

        public async Task<ExamRecordInfo> CorrectExam(ExamSubmitInfo submitInfo)
        {
            ExamRecordInfo recordInfo = GetRecordInfo(submitInfo);
            recordInfo.QuestionRecords = GetQuestionRecord(submitInfo.details);
            return _recordApp.AddRecord(recordInfo);
        }

        private ICollection<ExamQuestionRecordInfo> GetQuestionRecord(List<ExamDetailSubmitInfo> details)
        {
            List<ExamQuestionRecordInfo> questionRecords = new List<ExamQuestionRecordInfo>();
            foreach (ExamDetailSubmitInfo detail in details)
            {
                ExamQuestionRecordInfo recordInfo = new ExamQuestionRecordInfo();
                recordInfo.QuestionId = detail.questionId;
                recordInfo.QuestionScore = GeneralSolutionScore(detail);
                recordInfo.CreateUser = _loginUser;
                questionRecords.Add(recordInfo);
                recordInfo.SolutionRecords = GetSolutionRecord(detail);
            }
            return questionRecords;
        }

        private ICollection<ExamSolutionRecordInfo> GetSolutionRecord(ExamDetailSubmitInfo detail)
        {
            List<ExamSolutionRecordInfo> solutionRecords = new List<ExamSolutionRecordInfo>();
            foreach (int solutionId in detail.solutionId)
            {
                SolutionInfo solution = _questionApp.QuerySolution(p => p.QuestionId.Equals(detail.questionId) && p.SolutionId.Equals(solutionId));
                ExamSolutionRecordInfo recordInfo = new ExamSolutionRecordInfo();
                recordInfo.SolutionId = solutionId;
                recordInfo.IsCorrect = solution.IsCorrect;
                recordInfo.CreateUser = _loginUser;
                solutionRecords.Add(recordInfo);
            }
            return solutionRecords;
        }

        private ExamRecordInfo GetRecordInfo(ExamSubmitInfo submitInfo)
        {
            ExamRecordInfo recordInfo = new ExamRecordInfo();
            recordInfo.ExamPaperId = submitInfo.examId;
            recordInfo.LeftTime = submitInfo.leftTime;
            //string[] lt = recordInfo.LeftTime.Split(':');
            //DateTime beginTime = DateTime.Now.AddMinutes(StringUtil.GetInt(lt[0]) * -1).AddSeconds(StringUtil.GetInt(lt[1]) * -1);
            DateTime beginTime = GeneralUseTime(submitInfo);
            recordInfo.BeginTime = beginTime;
            recordInfo.ExamScore = GeneralScore(submitInfo);
            CheckIsPassed(recordInfo, submitInfo);
            recordInfo.CreateUser = _loginUser;
            return recordInfo;
        }

        private DateTime GeneralUseTime(ExamSubmitInfo submitInfo)
        {
            string leftTime = submitInfo.leftTime.Replace(':', '.');
            double time1 = StringUtil.GetDouble(leftTime);
            ExamPaperInfo examPaper = _examPaperApp.Load(submitInfo.examId);
            double time2 = StringUtil.GetDouble(string.Format("{0}.60", examPaper.ExamMinute-1)) - time1;
            string[] lt = time2.ToString("N2").Split('.');
            return DateTime.Now.AddMinutes(StringUtil.GetInt(lt[0]) * -1).AddSeconds(StringUtil.GetInt(lt[1]) * -1);
        }

        private void CheckIsPassed(ExamRecordInfo recordInfo, ExamSubmitInfo submitInfo)
        {
            if (recordInfo == null || submitInfo == null) { return; }
            ExamPaperInfo examPaper = _examPaperApp.Load(submitInfo.examId);
            double totalScore = examPaper.ExamScore;
            double passedScore = totalScore * 0.6;
            recordInfo.IsPassed = recordInfo.ExamScore >= passedScore;
        }

        private double GeneralScore(ExamSubmitInfo submitInfo)
        {
            double score = 0;
            foreach (ExamDetailSubmitInfo detail in submitInfo.details)
            {
                score += GeneralSolutionScore(detail);
            }
            return score;
        }

        private double GeneralSolutionScore(ExamDetailSubmitInfo detail)
        {
            double score = 0;
            foreach (int solutionId in detail.solutionId)
            {
                QuestionInfo question = _questionApp.Load(detail.questionId);
                SolutionInfo solution = _questionApp.QuerySolution(p => p.QuestionId.Equals(detail.questionId) && p.SolutionId.Equals(solutionId));
                if (solution.IsCorrect)
                {
                    score += solution.Score;
                }
                else
                {
                    score -= solution.Score;
                }
            }
            return score;
        }
    }
}
