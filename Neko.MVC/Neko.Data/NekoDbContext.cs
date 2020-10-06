using Microsoft.EntityFrameworkCore;
using Neko.Domain.Entities;
using Neko.Domain.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Data
{
    public class NekoDbContext : DbContext
    {
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<RoleInfo> Roles { get; set; }
        public DbSet<MenuInfo> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<QuestionInfo> Questions { get; set; }
        public DbSet<SolutionInfo> Solutions { get; set; }
        public DbSet<QuestionSolutionInfo> QuestionSolutions { get; set; }
        public DbSet<ExamPaperInfo> Exams { get; set; }
        public DbSet<ExamQuestionInfo> ExamQuestions { get; set; }
        public DbSet<ExamRecordInfo> ExamRecords { get; set; }
        public DbSet<ExamRecordQuestionDetailInfo> ExamRecordQuestions { get; set; }
        public DbSet<ExamRecordSolutionDetailInfo> ExamRecordSolutions { get; set; }

        public NekoDbContext(DbContextOptions<NekoDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new QuestionMap());
            modelBuilder.ApplyConfiguration(new SolutionMap());
            modelBuilder.ApplyConfiguration(new ExamMap());
            modelBuilder.ApplyConfiguration(new ExamRecordMap());
            modelBuilder.ApplyConfiguration(new ExamQuestionRecordMap());
            modelBuilder.ApplyConfiguration(new ExamSolutionRecordMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
