using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class SolutionMap : IEntityTypeConfiguration<SolutionInfo>
    {
        public void Configure(EntityTypeBuilder<SolutionInfo> builder)
        {
            builder.HasOne(p => p.CreateUser);
            builder.HasMany(p => p.Questions).WithOne(p => p.Solution).HasForeignKey(p => p.SolutionId);
        }
    }
}
