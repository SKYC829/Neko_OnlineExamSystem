using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class ExamRecordQuestionRepository : BaseRepository<ExamRecordQuestionDetailInfo>,IExamRecordQuestionRepository
    {
        public ExamRecordQuestionRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<ExamRecordQuestionDetailInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<ExamRecordQuestionDetailInfo, bool>> whereBy, Expression<Func<ExamRecordQuestionDetailInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }
    }
}
