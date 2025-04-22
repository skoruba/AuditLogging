using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skoruba.AuditLogging.Constants;
using Skoruba.AuditLogging.EntityFramework.DbContexts.Default;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.Host.Consts;
using Skoruba.AuditLogging.Host.Helpers.Authentication;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddProblemDetails();

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuditLogging(options =>
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
    .AddDefaultStore(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"),
            optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
    .AddDefaultAuditSink();

builder.Services.AddAuthentication(AuthenticationConsts.Scheme)
    .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(AuthenticationConsts.Scheme,
        options =>
        {
            options.Identity = new ClaimsIdentity(
            [
                new Claim(AuthenticationConsts.ClaimName, "bob"),
                            new Claim(AuthenticationConsts.ClaimSub, Guid.CreateVersion7().ToString()),
                            new Claim(AuthenticationConsts.ClaimRole, Guid.CreateVersion7().ToString()),
                            new Claim(AuthenticationConsts.ClaimRole, Guid.CreateVersion7().ToString())
            ], AuthenticationConsts.AuthenticationType, AuthenticationConsts.ClaimName, AuthenticationConsts.ClaimRole);
        });

builder.Services.AddControllersWithViews();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapOpenApi();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Note this will drop Authorization headers

app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();
app.MapDefaultControllerRoute();

await AutoCreateDatabase(app);

await app.RunAsync();

public partial class Program
{
    static async Task AutoCreateDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetRequiredService<DefaultAuditLoggingDbContext>();
            logger.LogInformation("Starting automatic database creation");
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("Database creation completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred creating the DB. {exceptionMessage}", ex.Message);
        }
    }
}
