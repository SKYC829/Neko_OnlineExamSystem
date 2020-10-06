using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neko.Domain.Interfaces
{
    public interface IQuestionRepository:IRepository<QuestionInfo>
    {
        IQueryable<SolutionInfo> QuerySolutions();

        IQueryable<SolutionInfo> QuerySolutions(int questionId);
    }
}
