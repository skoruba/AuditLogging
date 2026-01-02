using System;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TechAdvisor.AuditLogging.Constants;
using TechAdvisor.AuditLogging.EntityFramework.Extensions;
using TechAdvisor.AuditLogging.Host.Consts;
using TechAdvisor.AuditLogging.Host.Helpers.Authentication;

namespace TechAdvisor.AuditLogging.Host
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuditLogging(options =>
                {
                    options.Enabled = true;
                    options.UseDefaultAction = true;
                    options.UseDefaultSubject = true;
                    options.Source = typeof(Program).Assembly.GetName().Name;
                })
                .AddDefaultHttpEventData(subjectOptions =>
                    {
                        subjectOptions.SubjectIdentifierClaim = ClaimsConsts.Sub;
                        subjectOptions.SubjectNameClaim = ClaimsConsts.Name;
                    },
                    actionOptions =>
                    {
                        actionOptions.IncludeFormVariables = true;
                    })
                .AddDefaultStore(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext"),
                    optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
                .AddDefaultAuditSink();

            services.AddAuthentication(AuthenticationConsts.Scheme)
                .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(AuthenticationConsts.Scheme,
                    options =>
                    {
                        options.Identity = new ClaimsIdentity(new[]
                        {
                        new Claim(AuthenticationConsts.ClaimName, "bob"),
                        new Claim(AuthenticationConsts.ClaimSub, Guid.NewGuid().ToString()),
                        new Claim(AuthenticationConsts.ClaimRole, Guid.NewGuid().ToString()),
                        new Claim(AuthenticationConsts.ClaimRole, Guid.NewGuid().ToString())
                    }, AuthenticationConsts.AuthenticationType, AuthenticationConsts.ClaimName, AuthenticationConsts.ClaimRole);
                    });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapDefaultControllerRoute();
            });
        }
    }
}
