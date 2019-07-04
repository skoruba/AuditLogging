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
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Host.Consts;
using Skoruba.AuditLogging.Host.Helpers.Authentication;

namespace Skoruba.AuditLogging.Host
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
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
                    options.UseDefaultAction = true;
                    options.UseDefaultSubject = true;
                })
                .AddDefaultEventData()
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
