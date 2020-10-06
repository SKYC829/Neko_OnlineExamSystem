using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class ExamMap : IEntityTypeConfiguration<ExamPaperInfo>
    {
        public void Configure(EntityTypeBuilder<ExamPaperInfo> builder)
        {
            builder.HasOne(p => p.CreateUser);
            builder.HasMany(p => p.Questions).WithOne(p => p.Exam).HasForeignKey(p => p.ExamId);
        }
    }
}
