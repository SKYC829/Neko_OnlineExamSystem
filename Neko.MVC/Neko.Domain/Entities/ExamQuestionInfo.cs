using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("tb_ExamQuestions")]
    public class ExamQuestionInfo : DbEntity
    {
        public int ExamId { get; set; }

        public int RelationId { get; set; }
        public virtual ExamPaperInfo Exam { get; set; }

        public virtual QuestionInfo Questions { get; set; }
    }
}
