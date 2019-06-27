![Logo](docs/Images/Skoruba-ReadMe.png) 

# 🕊️ Skoruba.AuditLogging
> Simple audit logging for .NET Core with EntityFramework Core support

# How to install

```
dotnet add package Skoruba.AuditLogging.EntityFramework --version 1.0.0-beta2
```

# How to use

### Register default services
```
services.AddAuditLogging()
                .AddDefaultStore(options => options.UseSqlServer(
                    Configuration.GetConnectionString("MyConnectionString"),
                    optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
                .AddDefaultAuditSink();
```

### Usage in code

```
await _auditLogger.LogAsync(new ProductAddedEvent
            {
                Category = nameof(ProductAddedEvent),
                ProductDto = new ProductDto
                {
                    Name = "Coca Cola",
                    Category = "Drink"                    
                }
            });
```

### Full sample

**Startup.cs**:
```
var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuditCaller, AuditCaller>();

            services.AddAuditLogging()
                .AddDefaultStore(options => options.UseSqlServer(
                    Configuration.GetConnectionString("ApplicationDbContext"),
                    optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
                .AddDefaultAuditSink();
```

**AuditCaller.cs**:
```
public class AuditCaller : IAuditCaller
    {
        public AuditCaller(IHttpContextAccessor accessor)
        {
            SubjectIdentifier = accessor.HttpContext.User.FindFirst("sub")?.Value;
            SubjectName = accessor.HttpContext.User.FindFirst("name")?.Value;
        }

        public string SubjectName { get; set; }

        public string SubjectIdentifier { get; set; }
    }
```

**ProductAddedEvent.cs**
```
public class ProductAddedEvent : AuditEvent
    {
        public ProductDto ProductDto { get; set; }  
    }
```

# Acknowledgements

- This library is inspired by [EventService from IdentityServer4](https://github.com/IdentityServer/IdentityServer4).
