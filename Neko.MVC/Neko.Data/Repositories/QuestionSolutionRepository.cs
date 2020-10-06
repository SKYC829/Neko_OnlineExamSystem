using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class QuestionSolutionRepository : BaseRepository<QuestionSolutionInfo>, IQuestionSolutionRepository
    {
        public QuestionSolutionRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<QuestionSolutionInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<QuestionSolutionInfo, bool>> whereBy, Expression<Func<QuestionSolutionInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }

        public IQueryable<QuestionSolutionInfo> Query(Expression<Func<QuestionSolutionInfo, bool>> expression)
        {
            return All(expression).AsQueryable<QuestionSolutionInfo>();
        }
    }
}
