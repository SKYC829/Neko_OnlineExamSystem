using Microsoft.Extensions.Logging;
using Neko.App.Interfaces.Exam;
using Neko.App.Interfaces.Identity;
using Neko.App.Models.Exam;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Apps.ExamApps
{
    public class ExamRecordApp : IdentityAppBase, IExamRecordApp
    {
        private readonly IExamRecordRepository _recordRepository;
        private readonly IExamRecordQuestionRepository _questionRecordRepository;
        private readonly IExamRecordSolutionRepository _solutionRecordRepository;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IQuestionApp _questionApp;
        private readonly IExamPaperApp _examPaperApp;
        private readonly IUserApp _userApp;
        private readonly ILogger<ExamRecordApp> _log;

        public ExamRecordApp(IExamRecordRepository recordRepository, IExamRecordQuestionRepository questionRecordRepository, IExamRecordSolutionRepository solutionRecordRepository, ISolutionRepository solutionRepository, IQuestionApp questionApp, IExamPaperApp examPaperApp, IUserApp userApp, ILogger<ExamRecordApp> logger)
        {
            _recordRepository = recordRepository;
            _questionRecordRepository = questionRecordRepository;
            _solutionRecordRepository = solutionRecordRepository;
            _solutionRepository = solutionRepository;
            _questionApp = questionApp;
            _examPaperApp = examPaperApp;
            _userApp = userApp;
            _log = logger;
        }

        public ExamQuestionRecordInfo AddQuestionRecord(int recordId, ExamQuestionRecordInfo recordInfo)
        {
            try
            {
                DbModel.ExamRecordQuestionDetailInfo saveInfo = new DbModel.ExamRecordQuestionDetailInfo();
                saveInfo.RecordId = recordId;
                saveInfo.QuestionId = recordInfo.QuestionId;
                saveInfo.QuestionScore = recordInfo.QuestionScore;
                saveInfo.CreateUserId = recordInfo.CreateUser == null ? _loginUser.Id : recordInfo.CreateUser.Id;
                saveInfo.CreateDate = DateTime.Now;
                saveInfo = _questionRecordRepository.Create(saveInfo);
                recordInfo.Id = saveInfo.Id;
                foreach (ExamSolutionRecordInfo item in recordInfo.SolutionRecords)
                {
                    AddSolutionRecord(recordId, recordInfo.Id, item);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, nameof(AddQuestionRecord));
                throw;
            }
            return recordInfo;
        }

        public ExamRecordInfo AddRecord(ExamRecordInfo recordInfo)
        {
            try
            {
                DbModel.ExamRecordInfo saveInfo = new DbModel.ExamRecordInfo();
                saveInfo.ExamPaperId = recordInfo.ExamPaperId;
                saveInfo.ExamScore = recordInfo.ExamScore;
                saveInfo.IsPassed = recordInfo.IsPassed;
                saveInfo.LeftTime = recordInfo.LeftTime;
                saveInfo.CreateUserId = recordInfo.CreateUser == null ? _loginUser.Id : recordInfo.CreateUser.Id;
                saveInfo.CreateDate = DateTime.Now;
                saveInfo.BeginTime = recordInfo.BeginTime;
                saveInfo = _recordRepository.Create(saveInfo);
                recordInfo.Id = saveInfo.Id;
                recordInfo.CreateDate = saveInfo.CreateDate;
                recordInfo.CreateUserId = saveInfo.CreateUserId;
                recordInfo.ExamPaper = _examPaperApp.Load(saveInfo.ExamPaperId);
                foreach (ExamQuestionRecordInfo questionRecord in recordInfo.QuestionRecords)
                {
                    AddQuestionRecord(recordInfo.Id, questionRecord);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, nameof(AddRecord));
                throw;
            }
            return recordInfo;
        }

        public ExamSolutionRecordInfo AddSolutionRecord(int recordId, int detailId, ExamSolutionRecordInfo recordInfo)
        {
            try
            {
                DbModel.ExamRecordSolutionDetailInfo saveInfo = new DbModel.ExamRecordSolutionDetailInfo();
                saveInfo.DetailId = detailId;
                saveInfo.SolutionId = recordInfo.SolutionId;
                saveInfo.IsCorrect = recordInfo.IsCorrect;
                saveInfo.CreateUserId = recordInfo.CreateUser == null ? _loginUser.Id : recordInfo.CreateUser.Id;
                saveInfo.CreateDate = DateTime.Now;
                saveInfo = _solutionRecordRepository.Create(saveInfo);
                recordInfo.Id = saveInfo.Id;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, nameof(AddSolutionRecord));
                throw;
            }
            return recordInfo;
        }

        public async Task<ExamRecordInfo> Load(int recordId)
        {
            Task<ExamRecordInfo> result = Load(p => p.Id.Equals(recordId));
            return result.Result;
        }

        public async Task<ExamRecordInfo> Load(Expression<Func<DbModel.ExamRecordInfo, bool>> expression)
        {
            DbModel.ExamRecordInfo recordInfo = _recordRepository.Load(expression);
            ExamRecordInfo result = new ExamRecordInfo();
            result.Id = recordInfo.Id;
            result.ExamPaperId = recordInfo.ExamPaperId;
            result.ExamScore = recordInfo.ExamScore;
            result.IsPassed = recordInfo.IsPassed;
            result.LeftTime = recordInfo.LeftTime;
            result.BeginTime = recordInfo.BeginTime;
            result.CreateUserId = recordInfo.CreateUserId;
            result.CreateDate = recordInfo.CreateDate;
            result.CreateUser = _userApp.Load(result.CreateUserId.ToString());
            result.ExamPaper = _examPaperApp.Load(result.ExamPaperId);
            result.QuestionRecords = QueryRecordQuestion(result.Id).ToList();
            foreach (ExamQuestionRecordInfo questionRecord in result.QuestionRecords)
            {
                questionRecord.ExamRecord = result;
            }
            return result;
        }

        public IQueryable<ExamRecordInfo> QueryRecord()
        {
            IEnumerable<DbModel.ExamRecordInfo> examRecords = _recordRepository.All();
            List<ExamRecordInfo> results = new List<ExamRecordInfo>();
            foreach (DbModel.ExamRecordInfo record in examRecords)
            {
                ExamRecordInfo result = new ExamRecordInfo();
                result.Id = record.Id;
                result.ExamPaperId = record.ExamPaperId;
                result.ExamScore = record.ExamScore;
                result.IsPassed = record.IsPassed;
                result.LeftTime = record.LeftTime;
                result.BeginTime = record.BeginTime;
                result.CreateUserId = record.CreateUserId;
                result.CreateDate = record.CreateDate;
                results.Add(result);
            }
            foreach (ExamRecordInfo record in results)
            {
                record.CreateUser = _userApp.Load(record.CreateUserId.ToString());
                record.ExamPaper = _examPaperApp.Load(record.ExamPaperId);
                record.QuestionRecords = QueryRecordQuestion(record.Id).ToList();
                foreach (ExamQuestionRecordInfo questionRecord in record.QuestionRecords)
                {
                    questionRecord.ExamRecord = record;
                }
            }
            return results.AsQueryable();
        }

        public IQueryable<ExamRecordInfo> QueryRecordByUser(int userId)
        {
            return QueryRecordByUser(userId, null);
        }

        public IQueryable<ExamRecordInfo> QueryRecordByUser(int userId, Expression<Func<DbModel.ExamRecordInfo, bool>> expression)
        {
            IEnumerable<DbModel.ExamRecordInfo> examRecords = _recordRepository.All();
            if (userId > 0)
            {
                examRecords = examRecords.AsQueryable().Where(p => p.CreateUserId.Equals(userId));
            }
            if(expression != null)
            {
                examRecords = examRecords.AsQueryable().Where(expression);
            }
            List<ExamRecordInfo> results = new List<ExamRecordInfo>();
            foreach (DbModel.ExamRecordInfo record in examRecords)
            {
                ExamRecordInfo result = new ExamRecordInfo();
                result.Id = record.Id;
                result.ExamPaperId = record.ExamPaperId;
                result.ExamScore = record.ExamScore;
                result.IsPassed = record.IsPassed;
                result.LeftTime = record.LeftTime;
                result.BeginTime = record.BeginTime;
                result.CreateUserId = record.CreateUserId;
                result.CreateDate = record.CreateDate;
                results.Add(result);
            }
            foreach (ExamRecordInfo record in results)
            {
                record.CreateUser = _userApp.Load(record.CreateUserId.ToString());
                record.ExamPaper = _examPaperApp.Load(record.ExamPaperId);
                record.QuestionRecords = QueryRecordQuestion(record.Id).ToList();
                foreach (ExamQuestionRecordInfo questionRecord in record.QuestionRecords)
                {
                    questionRecord.ExamRecord = record;
                }
            }
            return results.AsQueryable();
        }

        public IQueryable<ExamQuestionRecordInfo> QueryRecordQuestion(int recordId)
        {
            return QueryRecordQuestion(recordId, null);
        }

        public IQueryable<ExamQuestionRecordInfo> QueryRecordQuestion(int recordId, Expression<Func<DbModel.ExamRecordQuestionDetailInfo, bool>> expression)
        {
            IQueryable<DbModel.ExamRecordQuestionDetailInfo> questionRecords = _questionRecordRepository.All().AsQueryable();
            if (recordId > 0)
            {
                questionRecords = questionRecords.Where(p => p.RecordId.Equals(recordId));
            }
            if(expression != null)
            {
                questionRecords = questionRecords.Where(expression);
            }
            List<ExamQuestionRecordInfo> results = new List<ExamQuestionRecordInfo>();
            foreach (DbModel.ExamRecordQuestionDetailInfo record in questionRecords)
            {
                ExamQuestionRecordInfo result = new ExamQuestionRecordInfo();
                result.Id = record.Id;
                result.RecordId = record.RecordId;
                result.QuestionId = record.QuestionId;
                result.QuestionScore = record.QuestionScore;
                result.CreateUserId = record.CreateUserId;
                result.CreateDate = record.CreateDate;
                results.Add(result);
            }
            foreach (ExamQuestionRecordInfo record in results)
            {
                record.CreateUser = _userApp.Load(record.CreateUserId.ToString());
                record.Question = _questionApp.Load(record.QuestionId);
                record.SolutionRecords = QueryRecordSolution(record.RecordId, record.Id).ToList();
                foreach (ExamSolutionRecordInfo solutionRecord in record.SolutionRecords)
                {
                    solutionRecord.QuestionRecord = record;
                }
            }
            return results.AsQueryable();
        }

        public IQueryable<ExamSolutionRecordInfo> QueryRecordSolution(int recordId, int detailId)
        {
            return QueryRecordSolution(recordId, detailId, null);
        }

        public IQueryable<ExamSolutionRecordInfo> QueryRecordSolution(int recordId, int detailId, Expression<Func<DbModel.ExamRecordSolutionDetailInfo, bool>> expression)
        {
            IQueryable<DbModel.ExamRecordSolutionDetailInfo> solutionRecords = _solutionRecordRepository.All().AsQueryable();
            if (recordId > 0)
            {
                solutionRecords = solutionRecords.Where(p => p.QuestionRecord.RecordId.Equals(recordId));
            }
            if (detailId > 0)
            {
                solutionRecords = solutionRecords.Where(p => p.DetailId.Equals(detailId));
            }
            if(expression != null)
            {
                solutionRecords = solutionRecords.Where(expression);
            }
            List<ExamSolutionRecordInfo> results = new List<ExamSolutionRecordInfo>();
            foreach (DbModel.ExamRecordSolutionDetailInfo record in solutionRecords)
            {
                ExamSolutionRecordInfo result = new ExamSolutionRecordInfo();
                result.Id = record.Id;
                result.SolutionId = record.SolutionId;
                result.IsCorrect = record.IsCorrect;
                result.CreateUserId = record.CreateUserId;
                result.CreateDate = record.CreateDate;
                results.Add(result);
            }
            foreach (ExamSolutionRecordInfo record in results)
            {
                record.CreateUser = _userApp.Load(record.CreateUserId.ToString());
                record.Solution = _questionApp.QuerySolution(p => p.SolutionId.Equals(record.SolutionId));
            }
            return results.AsQueryable();
        }
    }
}
