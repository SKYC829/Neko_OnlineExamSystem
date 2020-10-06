using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class QuestionSolutionMap : IEntityTypeConfiguration<QuestionSolutionInfo>
    {
        public void Configure(EntityTypeBuilder<QuestionSolutionInfo> builder)
        {
            //builder.HasOne(p => p.Question).WithMany(p => p.Solutions).HasForeignKey(p => p.QuestionId);
            //builder.HasOne(p => p.Solution).WithMany(p => p.Questions).HasForeignKey(p => p.SolutionId);
            //builder.HasMany(p => p.Exams).WithOne(p => p.QuestionSolution).HasForeignKey(p => p.RelationId);
        }
    }
}
