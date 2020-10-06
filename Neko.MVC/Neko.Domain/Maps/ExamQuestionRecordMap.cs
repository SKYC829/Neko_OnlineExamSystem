using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class ExamQuestionRecordMap : IEntityTypeConfiguration<ExamRecordQuestionDetailInfo>
    {
        public void Configure(EntityTypeBuilder<ExamRecordQuestionDetailInfo> builder)
        {
            builder.HasOne(p => p.CreateUser);
            builder.HasOne(p => p.Question);
            builder.HasMany(p => p.SolutionRecords).WithOne(p => p.QuestionRecord).HasForeignKey(p => p.DetailId);
        }
    }
}
