using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class ExamQuestionRepository : BaseRepository<ExamQuestionInfo>, IExamQuestionRepository
    {
        public ExamQuestionRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<ExamQuestionInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<ExamQuestionInfo, bool>> whereBy, Expression<Func<ExamQuestionInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
