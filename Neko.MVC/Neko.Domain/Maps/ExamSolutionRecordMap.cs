using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class ExamSolutionRecordMap : IEntityTypeConfiguration<ExamRecordSolutionDetailInfo>
    {
        public void Configure(EntityTypeBuilder<ExamRecordSolutionDetailInfo> builder)
        {
            builder.HasOne(p => p.CreateUser);
            builder.HasOne(p => p.Solution);
        }
    }
}
