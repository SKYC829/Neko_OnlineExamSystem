using Neko.Domain.Entities;
using Neko.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Neko.Data.Repositories
{
    public class SolutionRepository : BaseRepository<SolutionInfo>, ISolutionRepository
    {
        public SolutionRepository(NekoDbContext context) : base(context)
        {
        }

        public IQueryable<SolutionInfo> Query(int pageIndex, out int totalCount, out int totalPage, Expression<Func<SolutionInfo, bool>> whereBy, Expression<Func<SolutionInfo, int>> orderBy)
        {
            return Query(pageIndex, 20, out totalCount, out totalPage, whereBy, orderBy);
        }

        public IEnumerable<SolutionInfo> SaveList(IEnumerable<SolutionInfo> solutionList)
        {
            List<SolutionInfo> results = new List<SolutionInfo>();
            foreach (SolutionInfo solution in solutionList)
            {
                results.Add(Save(solution));
            }
            return results.AsQueryable();
        }
    }
}
