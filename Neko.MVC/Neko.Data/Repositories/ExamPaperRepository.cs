using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class ExamPaperRepository : BaseRepository<ExamPaperInfo>, IExamPaperRepository
    {
        public ExamPaperRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<ExamPaperInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<ExamPaperInfo, bool>> whereBy, Expression<Func<ExamPaperInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
