using Neko.App.Interfaces.Exam;
using Neko.App.Models.Exam;
using Neko.App.Models.Identity;
using Neko.Domain.Interfaces;
using Neko.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Apps.ExamApps
{
    public class QuestionApp : IdentityAppBase, IQuestionApp
    {
        private readonly IQuestionRepository _repository;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IQuestionSolutionRepository _relationRepository;
        public QuestionApp(IQuestionRepository repository, ISolutionRepository solutionRepository, IQuestionSolutionRepository relationRepository)
        {
            _repository = repository;
            _solutionRepository = solutionRepository;
            _relationRepository = relationRepository;
        }

        public QuestionInfo Load(int questionId)
        {
            DbModel.QuestionInfo question = InternalLoad(questionId);
            if (question == null)
            {
                return null;
            }
            QuestionInfo result = new QuestionInfo();
            result.Id = question.Id;
            result.Name = question.QuestionName;
            result.QuestionScore = question.QuestionScore;
            result.QuestionType = (QuestionType)question.QuestionType;
            result.Solutions = QuerySolutions(question.Solutions);
            return result;
        }

        internal DbModel.QuestionInfo InternalLoad(int questionId)
        {
            return _repository.Load(questionId);
        }

        public IQueryable<QuestionInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<DbModel.QuestionInfo, bool>> whereBy, Expression<Func<DbModel.QuestionInfo, int>> orderBy)
        {
            IEnumerable<DbModel.QuestionInfo> dbRes = _repository.Query(pageIndex, out totalCount, out totalPage, whereBy, orderBy);
            List<QuestionInfo> results = new List<QuestionInfo>();
            foreach (DbModel.QuestionInfo question in dbRes)
            {
                QuestionInfo result = new QuestionInfo();
                result.Id = question.Id;
                result.Name = question.QuestionName;
                result.QuestionScore = question.QuestionScore;
                result.QuestionType = (QuestionType)question.QuestionType;
                //result.Solutions = QuerySolutions(question.Solutions);
                results.Add(result);
            }
            foreach (QuestionInfo item in results)
            {
                item.Solutions = QuerySolutions(item.Id);
            }
            return results.AsQueryable<QuestionInfo>();
        }

        public IQueryable<SolutionInfo> QuerySolutions()
        {
            IQueryable<DbModel.SolutionInfo> dbRes = _repository.QuerySolutions();
            List<SolutionInfo> results = new List<SolutionInfo>();
            foreach (DbModel.SolutionInfo solution in dbRes)
            {
                SolutionInfo result = new SolutionInfo();
                result.Id = solution.Id;
                result.Name = solution.Name;
                result.IsCorrect = solution.IsCorrect;
                results.Add(result);
            }
            return results.AsQueryable<SolutionInfo>();
        }

        public IQueryable<SolutionInfo> QuerySolutions(int questionId)
        {
            IQueryable<DbModel.QuestionSolutionInfo> relations = _relationRepository.Query(p => p.QuestionId.Equals(questionId));
            return QuerySolutions(relations);
        }

        public IQueryable<SolutionInfo> QuerySolutions(IEnumerable<DbModel.QuestionSolutionInfo> relationInfo)
        {
            List<SolutionInfo> results = new List<SolutionInfo>();
            foreach (DbModel.QuestionSolutionInfo relation in relationInfo)
            {
                SolutionInfo result = new SolutionInfo();
                result.RelationId = relation.Id;
                result.Name = relation.Solution.Name;
                result.Score = relation.Solution.SolutionSocre;
                result.IsCorrect = relation.Solution.IsCorrect;
                result.Id = relation.SolutionId;
                results.Add(result);
            }
            return results.AsQueryable<SolutionInfo>();
        }

        public SolutionInfo QuerySolution(Expression<Func<DbModel.QuestionSolutionInfo,bool>> expression)
        {
            if(expression == null) { return null; }
            DbModel.QuestionSolutionInfo relation = _relationRepository.Load(expression);
            SolutionInfo result = new SolutionInfo();
            result.RelationId = relation.Id;
            result.Name = relation.Solution.Name;
            result.Score = relation.Solution.SolutionSocre;
            result.IsCorrect = relation.Solution.IsCorrect;
            result.Id = relation.SolutionId;
            return result;
        }

        private DbModel.QuestionSolutionInfo GetRelation(QuestionInfo question,SolutionInfo solution)
        {
            return _relationRepository.Load(p => p.QuestionId.Equals(question.Id) && p.SolutionId.Equals(solution.Id));
        }

        public bool Remove(int questionId)
        {
            QuestionInfo questionInfo = Load(questionId);
            return Remove(questionInfo);
        }

        public bool Remove(QuestionInfo questionInfo)
        {
            try
            {
                RemoveRelation(questionInfo, questionInfo.Solutions.ToList());
                _repository.Remove(questionInfo.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public QuestionInfo Save(QuestionInfo questionInfo)
        {
            if (questionInfo.Id <= 0)
            {
                return CreateQuestion(questionInfo);
            }
            else
            {
                return UpdateQuestion(questionInfo);
            }
        }

        private QuestionInfo CreateQuestion(QuestionInfo questionInfo)
        {
            //创建的时候答案肯定是没有的，所以不用判断relation
            DbModel.QuestionInfo question = new DbModel.QuestionInfo();
            question.QuestionName = questionInfo.Name;
            question.QuestionScore = questionInfo.QuestionScore;
            question.QuestionType = (DbModel.QuestionType)questionInfo.QuestionType;
            question.CreateUserId = _loginUser.Id;
            question.CreateDate = DateTime.Now;
            question = _repository.Save(question);
            questionInfo.Id = question.Id;
            //保存答案
            if (questionInfo.Solutions != null && questionInfo.Solutions.Count() > 0)
            {
                SaveRelation(questionInfo);
            }
            return questionInfo;
        }

        private QuestionInfo UpdateQuestion(QuestionInfo questionInfo)
        {
            //先加载出来数据库当前这个问题的数据
            //保存原有的答案
            //对比现在的答案得到已删除的答案
            //更新问题数据
            DbModel.QuestionInfo question = InternalLoad(questionInfo.Id);
            question.QuestionName = questionInfo.Name;
            question.QuestionScore = questionInfo.QuestionScore;
            question.QuestionType = (DbModel.QuestionType)questionInfo.QuestionType;
            question = _repository.Save(question);
            //保存问题答案
            if (questionInfo.Solutions != null && questionInfo.Solutions.Count() > 0)
            {
                IEnumerable<SolutionInfo> oldSolutions = QuerySolutions(questionInfo.Id);
                List<SolutionInfo> deleteSolutions = new List<SolutionInfo>();
                foreach (SolutionInfo item in oldSolutions)
                {
                    SolutionInfo solution = questionInfo.Solutions.FirstOrDefault(p => p.Name.Equals(item.Name));
                    if (solution == null)
                    {
                        deleteSolutions.Add(item);
                    }
                    else
                    {
                        solution.Id = item.Id;
                        solution.RelationId = item.RelationId;
                    }
                }
                SaveRelation(questionInfo);
                RemoveRelation(questionInfo, deleteSolutions);
            }
            return questionInfo;
        }

        private void SaveRelation(QuestionInfo questionInfo)
        {
            //保存答案
            if (questionInfo.Solutions != null && questionInfo.Solutions.Count() > 0)
            {
                //循环保存答案
                foreach (SolutionInfo solution in questionInfo.Solutions)
                {
                    DbModel.SolutionInfo so = null;
                    if(solution.Id <= 0)
                    {
                        so = new DbModel.SolutionInfo();
                        so.CreateUserId = _loginUser.Id;
                        so.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        so = _solutionRepository.Load(solution.Id);
                        so.Id = solution.Id;
                    }
                    so.Name = solution.Name;
                    so.IsCorrect = solution.IsCorrect;
                    so.SolutionSocre = solution.Score;
                    so = _solutionRepository.Save(so);
                    _solutionRepository.Commit();
                    solution.Id = so.Id;
                }
                //循环保存答案和问题的关联关系
                foreach (SolutionInfo solution in questionInfo.Solutions)
                {
                    DbModel.QuestionSolutionInfo relationInfo = null;
                    if (solution.RelationId != 0)
                    {
                        relationInfo = _relationRepository.Load(solution.RelationId);
                    }
                    else
                    {
                        relationInfo = new DbModel.QuestionSolutionInfo();
                    }
                    relationInfo.QuestionId = questionInfo.Id;
                    relationInfo.SolutionId = solution.Id;
                    _relationRepository.Save(relationInfo);
                }
            }
        }

        private void RemoveRelation(QuestionInfo question,List<SolutionInfo> removeList)
        {
            foreach (SolutionInfo solution in removeList)
            {
                DbModel.QuestionSolutionInfo relationInfo = GetRelation(question, solution);
                _relationRepository.Remove(relationInfo);
            }
            foreach (SolutionInfo solution in removeList)
            {
                _solutionRepository.Remove(solution.Id);
            }
        }

        public IQueryable<QuestionInfo> Query(Expression<Func<DbModel.QuestionInfo, bool>> expression)
        {
            IEnumerable<DbModel.QuestionInfo> questions = _repository.All(expression);
            List<QuestionInfo> results = new List<QuestionInfo>();
            foreach (DbModel.QuestionInfo question in questions)
            {
                QuestionInfo result = new QuestionInfo();
                result.Id = question.Id;
                result.Name = question.QuestionName;
                result.QuestionScore = question.QuestionScore;
                result.QuestionType = (QuestionType)question.QuestionType;
                //result.Solutions = QuerySolutions(question.Solutions);
                results.Add(result);
            }
            foreach (QuestionInfo item in results)
            {
                item.Solutions = QuerySolutions(item.Id);
            }
            return results.AsQueryable<QuestionInfo>();
        }
    }
}
