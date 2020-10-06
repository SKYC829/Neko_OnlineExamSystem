using Microsoft.Extensions.Logging;
using Neko.App.Interfaces.Exam;
using Neko.App.Models.Exam;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Apps.ExamApps
{
    public class ExamPaperApp : IdentityAppBase, IExamPaperApp
    {
        private readonly IExamPaperRepository _repository;
        private readonly IExamQuestionRepository _relationRepository;
        private readonly IQuestionApp _questionApp;
        private readonly ILogger<ExamPaperApp> _log;
        public ExamPaperApp(IExamPaperRepository repository, IExamQuestionRepository relationRepository, IQuestionApp questionApp)
        {
            _repository = repository;
            _relationRepository = relationRepository;
            _questionApp = questionApp;
        }

        public ExamPaperApp(IExamPaperRepository repository, IExamQuestionRepository relationRepository, IQuestionApp questionApp, ILogger<ExamPaperApp> logger) :this(repository, relationRepository, questionApp)
        {
            _log = logger;
        }
        public ExamPaperInfo Load(int paperId)
        {
            DbModel.ExamPaperInfo paperInfo = InternalLoad(paperId);
            if (paperInfo == null)
            {
                return null;
            }
            ExamPaperInfo result = new ExamPaperInfo();
            result.Id = paperInfo.Id;
            result.Name = paperInfo.ExamName;
            result.ExamMinute = paperInfo.ExamMinute;
            result.ExamScore = paperInfo.ExamScore;
            result.QuestionRank = paperInfo.QuestionNums;
            result.ExamDateFrom = paperInfo.ExamDateFrom;
            result.ExamDateTo = paperInfo.ExamDateTo;
            result.Remark = paperInfo.Remark;
            result.Questions = QueryQuestion(result.Id).ToList();
            result.QuestionIds = new List<int>();
            foreach (QuestionInfo question in result.Questions)
            {
                result.QuestionIds.Add(question.Id);
            }
            return result;
        }

        public IQueryable<ExamPaperInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<DbModel.ExamPaperInfo, bool>> whereBy, Expression<Func<DbModel.ExamPaperInfo, int>> orderBy)
        {
            IEnumerable<DbModel.ExamPaperInfo> paperInfos = _repository.Query(pageIndex, out totalCount, out totalPage, whereBy, orderBy);
            List<ExamPaperInfo> results = new List<ExamPaperInfo>();
            foreach (DbModel.ExamPaperInfo paperInfo in paperInfos)
            {
                ExamPaperInfo result = new ExamPaperInfo();
                result.Id = paperInfo.Id;
                result.Name = paperInfo.ExamName;
                result.ExamMinute = paperInfo.ExamMinute;
                result.ExamScore = paperInfo.ExamScore;
                result.ExamDateFrom = paperInfo.ExamDateFrom;
                result.ExamDateTo = paperInfo.ExamDateTo;
                result.Remark = paperInfo.Remark;
                result.QuestionRank = paperInfo.QuestionNums;
                results.Add(result);
            }
            foreach (ExamPaperInfo paperInfo in results)
            {
                paperInfo.Questions = QueryQuestion(paperInfo.Id).ToList();
                paperInfo.QuestionIds = new List<int>();
                foreach (QuestionInfo question in paperInfo.Questions)
                {
                    paperInfo.QuestionIds.Add(question.Id);
                }
                paperInfo.QuestionsNum = paperInfo.Questions.Count;
            }
            return results.AsQueryable();
        }

        public IQueryable<QuestionInfo> QueryQuestion(int paperId)
        {
            IQueryable<DbModel.ExamQuestionInfo> relations = QueryRelation(paperId);
            List<QuestionInfo> result = new List<QuestionInfo>();
            foreach (DbModel.ExamQuestionInfo relation in relations)
            {
                QuestionInfo question = _questionApp.Load(relation.RelationId);
                result.Add(question);
            }
            return result.AsQueryable();
        }

        public IQueryable<DbModel.ExamQuestionInfo> QueryRelation(int paperId)
        {
            return QueryRelation(p => p.ExamId.Equals(paperId));
        }

        private IQueryable<DbModel.ExamQuestionInfo> QueryRelation(Expression<Func<DbModel.ExamQuestionInfo, bool>> expression)
        {
            return _relationRepository.All(expression).AsQueryable();
        }

        public bool Remove(int paperId)
        {
            ExamPaperInfo paperInfo = Load(paperId);
            return Remove(paperInfo);
        }

        public bool Remove(ExamPaperInfo paperInfo)
        {
            try
            {
                RemoveRelation(paperInfo, paperInfo.Questions.ToList());
                _repository.Remove(paperInfo.Id);
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError("删除考卷出错:{0}.", ex);
                return false;
            }
        }

        public EditExamPaperInfo Save(EditExamPaperInfo paperInfo)
        {
            EditExamPaperInfo result = null;
            if (paperInfo.Id <= 0)
            {
                result = CreatePaper(paperInfo);
            }
            else
            {
                result = UpdatePaper(paperInfo);
            }
            return result;
        }

        internal DbModel.ExamPaperInfo InternalLoad(int paperId)
        {
            return _repository.Load(paperId);
        }

        internal void RemoveRelation(EditExamPaperInfo paperInfo,List<QuestionInfo> questionInfo)
        {
            foreach (QuestionInfo question in questionInfo)
            {
                IQueryable<DbModel.ExamQuestionInfo> relations = QueryRelation(p => p.ExamId.Equals(paperInfo.Id) && p.RelationId.Equals(question.Id));
                foreach (DbModel.ExamQuestionInfo relation in relations)
                {
                    _relationRepository.Remove(relation);
                }
            }
        }

        internal void SaveRelation(EditExamPaperInfo paperInfo)
        {
            if(paperInfo.QuestionIds!=null && paperInfo.QuestionIds.Count > 0)
            {
                for (int i = 0; i < paperInfo.QuestionIds.Count; i++)
                {
                    DbModel.ExamQuestionInfo relation = null;
                    relation = _relationRepository.Load(p => p.ExamId.Equals(paperInfo.Id) && p.RelationId.Equals(paperInfo.QuestionIds.ElementAt(i)));
                    if(relation == null)
                    {
                        relation = new DbModel.ExamQuestionInfo();
                    }
                    relation.ExamId = paperInfo.Id;
                    relation.RelationId = paperInfo.QuestionIds.ElementAt(i);
                    _relationRepository.Save(relation);
                }
            }
        }

        private EditExamPaperInfo CreatePaper(EditExamPaperInfo paperInfo)
        {
            DbModel.ExamPaperInfo createInfo = new DbModel.ExamPaperInfo();
            createInfo.ExamName = paperInfo.Name;
            createInfo.ExamMinute = paperInfo.ExamMinute;
            createInfo.ExamScore = paperInfo.ExamScore;
            createInfo.QuestionNums = paperInfo.QuestionRank;
            createInfo.ExamDateFrom = paperInfo.ExamDateFrom;
            createInfo.ExamDateTo = paperInfo.ExamDateTo;
            createInfo.Remark = paperInfo.Remark;
            createInfo.CreateUserId = _loginUser.Id;
            createInfo.CreateDate = DateTime.Now;
            createInfo = _repository.Save(createInfo);
            paperInfo.Id = createInfo.Id;
            if(paperInfo.QuestionIds != null && paperInfo.QuestionIds.Count > 0)
            {
                SaveRelation(paperInfo);
            }
            return paperInfo;
        }

        private EditExamPaperInfo UpdatePaper(EditExamPaperInfo paperInfo)
        {
            DbModel.ExamPaperInfo updateInfo = InternalLoad(paperInfo.Id);
            updateInfo.ExamName = paperInfo.Name;
            updateInfo.ExamMinute = paperInfo.ExamMinute;
            updateInfo.ExamScore = paperInfo.ExamScore;
            updateInfo.QuestionNums = paperInfo.QuestionRank;
            updateInfo.ExamDateFrom = paperInfo.ExamDateFrom;
            updateInfo.ExamDateTo = paperInfo.ExamDateTo;
            updateInfo.Remark = paperInfo.Remark;
            _repository.Save(updateInfo);
            if(paperInfo.QuestionIds != null && paperInfo.QuestionIds.Count > 0)
            {
                IEnumerable<QuestionInfo> oldQuestions = QueryQuestion(paperInfo.Id);
                List<QuestionInfo> deleteQuestions = new List<QuestionInfo>();
                foreach (QuestionInfo question in oldQuestions)
                {
                    if (!paperInfo.QuestionIds.Contains(question.Id))
                    {
                        deleteQuestions.Add(question);
                    }
                }
                SaveRelation(paperInfo);
                RemoveRelation(paperInfo, deleteQuestions);
            }
            return paperInfo;
        }

        public IQueryable<ExamPaperInfo> Query()
        {
            return Query(null);
        }

        public IQueryable<ExamPaperInfo> Query(Expression<Func<DbModel.ExamPaperInfo, bool>> expression)
        {
            IEnumerable<DbModel.ExamPaperInfo> paperInfos = null;
            if (expression == null)
            {
                paperInfos = _repository.All();
            }
            else
            {
                paperInfos = _repository.All(expression);
            }
            List<ExamPaperInfo> results = new List<ExamPaperInfo>();
            foreach (DbModel.ExamPaperInfo paperInfo in paperInfos)
            {
                ExamPaperInfo result = new ExamPaperInfo();
                result.Id = paperInfo.Id;
                result.Name = paperInfo.ExamName;
                result.ExamMinute = paperInfo.ExamMinute;
                result.ExamScore = paperInfo.ExamScore;
                result.ExamDateFrom = paperInfo.ExamDateFrom;
                result.ExamDateTo = paperInfo.ExamDateTo;
                result.Remark = paperInfo.Remark;
                result.QuestionRank = paperInfo.QuestionNums;
                results.Add(result);
            }
            foreach (ExamPaperInfo paperInfo in results)
            {
                paperInfo.Questions = QueryQuestion(paperInfo.Id).ToList();
                paperInfo.QuestionIds = new List<int>();
                foreach (QuestionInfo question in paperInfo.Questions)
                {
                    paperInfo.QuestionIds.Add(question.Id);
                }
                paperInfo.QuestionsNum = paperInfo.Questions.Count;
            }
            return results.AsQueryable();
        }
    }
}
