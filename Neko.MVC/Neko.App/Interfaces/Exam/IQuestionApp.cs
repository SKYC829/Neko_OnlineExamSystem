using Neko.App.Models.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Interfaces.Exam
{
    public interface IQuestionApp
    {
        IQueryable<SolutionInfo> QuerySolutions();

        IQueryable<SolutionInfo> QuerySolutions(int questionId);

        SolutionInfo QuerySolution(Expression<Func<DbModel.QuestionSolutionInfo,bool>> expression);

        IQueryable<QuestionInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<DbModel.QuestionInfo, bool>> whereBy, Expression<Func<DbModel.QuestionInfo, int>> orderBy);

        IQueryable<QuestionInfo> Query(Expression<Func<DbModel.QuestionInfo, bool>> expression);

        QuestionInfo Save(QuestionInfo questionInfo);

        QuestionInfo Load(int questionId);

        bool Remove(int questionId);

        bool Remove(QuestionInfo questionInfo);
    }
}
