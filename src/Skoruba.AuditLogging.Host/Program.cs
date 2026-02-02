using System;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skoruba.AuditLogging.Constants;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.Host.Consts;
using Skoruba.AuditLogging.Host.Helpers.Authentication;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddHttpContextAccessor();

var migrationsAssembly = Assembly.GetExecutingAssembly().GetName().Name;

builder.Services.AddAuditLogging(options =>
    {
        options.Enabled = true;
        options.UseDefaultAction = true;
        options.UseDefaultSubject = true;
        options.Source = Assembly.GetExecutingAssembly().GetName().Name;
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
    .AddDefaultStore(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"),
        optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
    .AddDefaultAuditSink();

builder.Services.AddAuthentication(AuthenticationConsts.Scheme)
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

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();
