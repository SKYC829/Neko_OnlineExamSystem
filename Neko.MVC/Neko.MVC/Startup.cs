using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neko.App.Apps.ExamApps;
using Neko.App.Apps.IdentityApps;
using Neko.App.Interfaces.Exam;
using Neko.App.Interfaces.Identity;
using Neko.Data;
using Neko.Data.Repositories;
using Neko.Domain.Interfaces;

namespace Neko.MVC
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
            services.AddDbContextPool<NekoDbContext>(op =>
            {
                op.UseMySQL(_configuration.GetConnectionString("Mysql"));
                op.UseLazyLoadingProxies();
            });
            services.AddSession();
            #region “¿¿µ◊¢»Îµƒ…Ë÷√
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserApp, UserApp>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleApp, RoleApp>();

            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionApp, QuestionApp>();

            services.AddScoped<ISolutionRepository, SolutionRepository>();

            services.AddScoped<IQuestionSolutionRepository, QuestionSolutionRepository>();

            services.AddScoped<IExamPaperRepository, ExamPaperRepository>();
            services.AddScoped<IExamPaperApp, ExamPaperApp>();

            services.AddScoped<IExamQuestionRepository, ExamQuestionRepository>();

            services.AddScoped<IExamRecordRepository, ExamRecordRepository>();
            services.AddScoped<IExamRecordApp, ExamRecordApp>();

            services.AddScoped<IExamRecordQuestionRepository, ExamRecordQuestionRepository>();

            services.AddScoped<IExamRecordSolutionRepository, ExamRecordSolutionRepository>();

            services.AddScoped<ICorrectExamApp, CorrectExamApp>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");

                //endpoints.MapControllerRoute(
                //    name:"identity",
                //    pattern:"{Identity}/{controller}/{action}/{id?}");
            });
            if (env.IsDevelopment())
            {
                DbSeed.InitDefaultData(app.ApplicationServices);
            }
        }
    }
}
