using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class RoleMap : IEntityTypeConfiguration<RoleInfo>
    {
        public void Configure(EntityTypeBuilder<RoleInfo> builder)
        {
            //builder.OwnsOne(p => p.CreateUser);
            builder.HasOne(p => p.CreateUser);
            //builder.HasMany(p => p.Users).WithOne(p => p.Role).HasForeignKey(p => p.RoleId);
            builder.HasMany(p => p.Menus).WithOne(p => p.Role).HasForeignKey(p => p.RoleId);
        }
    }
}
