using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Neko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Util;

namespace Neko.Data
{
    public static class DbSeed
    {
        public static async void InitDefaultData(IServiceProvider provider)
        {
            using (var serviceScope = provider.CreateScope())
            {
                using (var context = new NekoDbContext(serviceScope.ServiceProvider.GetRequiredService<DbContextOptions<NekoDbContext>>()))
                {
                    context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0");
                    if (!context.Roles.Any())
                    {
                        context.Roles.AddRange
                        (
                            new RoleInfo
                            {
                                Id = 1,
                                RoleName = "管理员",
                                RoleType = RoleType.Admin,
                                IsRemove = false,
                                Remark = "管理员角色",
                                //CreateUserId = 1,
                                CreateDate = DateTime.Now
                            },
                            new RoleInfo
                            {
                                Id = 2,
                                RoleName = "教师",
                                RoleType = RoleType.Teacher,
                                IsRemove = false,
                                Remark = "教师角色",
                                //CreateUserId = 1,
                                CreateDate = DateTime.Now
                            },
                            new RoleInfo
                            {
                                Id = 3,
                                RoleName = "学生",
                                RoleType = RoleType.Student,
                                IsRemove = false,
                                Remark = "学生角色",
                                //CreateUserId = 1,
                                CreateDate = DateTime.Now
                            }
                        );
                    }

                    if (!context.Users.Any())
                    {
                        context.Users.Add(new UserInfo
                        {
                            Id = 1,
                            WorkId = "0001",
                            UserName = "Admin",
                            Password = "123456",
                            Password_Hash = "123456".EncryptMD5(),
                            RoleId = 1,
                            IsRemove = false,
                            IsLock = false,
                            Remark = "超级管理员",
                            CreateUserId = 1,
                            CreateDate = DateTime.Now
                        });
                    }


                    if (!context.Menus.Any())
                    {
                        context.Menus.AddRange
                        (
                            new MenuInfo
                            {
                                Id = 1,
                                ParentId = 0,
                                MenuIndex = 0,
                                MenuName = "考试管理"
                            },
                            new MenuInfo
                            {
                                Id = 2,
                                ParentId = 1,
                                MenuIndex = 0,
                                MenuName = "在线考试"
                            },
                            new MenuInfo
                            {
                                Id = 3,
                                ParentId = 1,
                                MenuIndex = 0,
                                MenuName = "历史答卷"
                            },
                            new MenuInfo
                            {
                                Id = 4,
                                ParentId = 0,
                                MenuIndex = 0,
                                MenuName = "考卷管理"
                            },
                            new MenuInfo
                            {
                                Id = 5,
                                ParentId = 4,
                                MenuIndex = 0,
                                MenuName = "考卷列表"
                            },
                            new MenuInfo
                            {
                                Id = 6,
                                ParentId = 4,
                                MenuIndex = 0,
                                MenuName = "题目列表"
                            },
                            new MenuInfo
                            {
                                Id = 7,
                                ParentId = 0,
                                MenuIndex = 0,
                                MenuName = "系统设置"
                            },
                            new MenuInfo
                            {
                                Id = 8,
                                ParentId = 7,
                                MenuIndex = 0,
                                MenuName = "用户管理"
                            },
                            new MenuInfo
                            {
                                Id = 9,
                                ParentId = 7,
                                MenuIndex = 0,
                                MenuName = "角色管理"
                            }
                        );
                    }
                    await context.SaveChangesAsync();
                    context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1");
                }
            }
        }
    }
}
