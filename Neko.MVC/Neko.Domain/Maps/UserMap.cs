using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class UserMap : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasOne(p => p.Role).WithMany(p => p.Users).HasForeignKey(p => p.RoleId);

            builder.HasMany(p => p.ExamRecords).WithOne(p => p.CreateUser).HasForeignKey(p => p.CreateUserId);
        }
    }
}
