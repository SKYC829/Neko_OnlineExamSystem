using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.App.Models.Exam
{
    public class SolutionInfo
    {
        public int Id { get; set; }

        public int RelationId { get; set; }

        public string Name { get; set; }

        public double Score { get; set; }

        public bool IsCorrect { get; set; }
    }
}
