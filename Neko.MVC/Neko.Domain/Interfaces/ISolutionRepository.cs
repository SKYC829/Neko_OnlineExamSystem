using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Interfaces
{
    public interface ISolutionRepository: IRepository<SolutionInfo>
    {
        IEnumerable<SolutionInfo> SaveList(IEnumerable<SolutionInfo> solutionList);
    }
}
