using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_QuestionSolutions")]
    public class QuestionSolutionInfo:DbEntity
    {
        public int QuestionId { get; set; }

        public int SolutionId { get; set; }

        public virtual QuestionInfo Question { get; set; }

        public virtual SolutionInfo Solution { get; set; }
    }
}
