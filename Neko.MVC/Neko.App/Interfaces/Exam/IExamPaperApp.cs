using Neko.App.Models.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DbModel = Neko.Domain.Entities;

namespace Neko.App.Interfaces.Exam
{
    public interface IExamPaperApp
    {
        IQueryable<ExamPaperInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<DbModel.ExamPaperInfo, bool>> whereBy, Expression<Func<DbModel.ExamPaperInfo, int>> orderBy);

        IQueryable<ExamPaperInfo> Query();

        IQueryable<ExamPaperInfo> Query(Expression<Func<DbModel.ExamPaperInfo, bool>> expression);

        EditExamPaperInfo Save(EditExamPaperInfo paperInfo);

        ExamPaperInfo Load(int paperId);

        IQueryable<DbModel.ExamQuestionInfo> QueryRelation(int paperId);

        IQueryable<QuestionInfo> QueryQuestion(int paperId);

        bool Remove(int paperId);

        bool Remove(ExamPaperInfo paperInfo);

    }
}
