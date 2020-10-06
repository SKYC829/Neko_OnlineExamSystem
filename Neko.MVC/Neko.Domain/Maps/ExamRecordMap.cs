using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class ExamRecordMap : IEntityTypeConfiguration<ExamRecordInfo>
    {
        public void Configure(EntityTypeBuilder<ExamRecordInfo> builder)
        {
            builder.HasOne(p => p.CreateUser);
            builder.HasOne(p => p.ExamPaper);
            builder.HasMany(p => p.QuestionRecords).WithOne(p => p.ExamRecord).HasForeignKey(p => p.RecordId);
        }
    }
}
