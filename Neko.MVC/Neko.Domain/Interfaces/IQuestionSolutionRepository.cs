using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Domain.Interfaces
{
    public interface IQuestionSolutionRepository : IRepository<QuestionSolutionInfo>
    {
        IQueryable<QuestionSolutionInfo> Query(Expression<Func<QuestionSolutionInfo, bool>> expression);
    }
}
