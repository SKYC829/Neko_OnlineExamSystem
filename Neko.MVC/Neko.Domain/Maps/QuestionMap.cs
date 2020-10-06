using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class QuestionMap : IEntityTypeConfiguration<QuestionInfo>
    {
        public void Configure(EntityTypeBuilder<QuestionInfo> builder)
        {
            builder.HasOne(p => p.CreateUser);
            builder.HasMany(p => p.Solutions).WithOne(p => p.Question).HasForeignKey(p => p.QuestionId);
            builder.HasMany(p => p.Exams).WithOne(p => p.Questions).HasForeignKey(p => p.RelationId);
        }
    }
}
