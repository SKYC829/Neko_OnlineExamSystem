using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Maps
{
    public class RoleMenuMap : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasNoKey();
            builder.HasOne(p => p.Role).WithMany(p => p.Menus).HasForeignKey(p=>p.RoleId);
        }
    }
}
