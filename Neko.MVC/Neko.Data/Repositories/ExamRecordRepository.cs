using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class ExamRecordRepository : BaseRepository<ExamRecordInfo>, IExamRecordRepository
    {
        public ExamRecordRepository(NekoDbContext dbContext):base(dbContext)
        {

        }

        public IQueryable<ExamRecordInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<ExamRecordInfo, bool>> whereBy, Expression<Func<ExamRecordInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
