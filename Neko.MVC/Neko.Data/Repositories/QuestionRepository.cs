using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class QuestionRepository : BaseRepository<QuestionInfo>, IQuestionRepository
    {
        public QuestionRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<QuestionInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<QuestionInfo, bool>> whereBy, Expression<Func<QuestionInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }

        public IQueryable<SolutionInfo> QuerySolutions()
        {
            return _dbContext.Set<SolutionInfo>();
        }

        public IQueryable<SolutionInfo> QuerySolutions(int questionId)
        {
            var solus = _dbContext.Set<QuestionSolutionInfo>().Where(p => p.QuestionId.Equals(questionId));
            var res = from t in solus select t.Solution;
            return res;
        }
    }
}
