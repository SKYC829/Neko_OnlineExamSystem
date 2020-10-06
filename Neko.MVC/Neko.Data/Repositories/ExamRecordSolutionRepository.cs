using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class ExamRecordSolutionRepository : BaseRepository<ExamRecordSolutionDetailInfo>,IExamRecordSolutionRepository
    {
        public ExamRecordSolutionRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<ExamRecordSolutionDetailInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<ExamRecordSolutionDetailInfo, bool>> whereBy, Expression<Func<ExamRecordSolutionDetailInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
