![Logo](docs/Images/Skoruba-ReadMe.png) 

# 🕊️ Skoruba.AuditLogging
> Simple audit logging for .NET Core with EntityFramework Core support

# How to install

```
dotnet add package Skoruba.AuditLogging.EntityFramework --version 1.0.0-beta3
```

# How to use

### Register default services
```
services.AddAuditLogging(options =>
                {
                    options.UseDefaultAction = true;
                    options.UseDefaultSubject = true;
                })
                .AddDefaultEventData()
                .AddDefaultStore(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext"),
                    optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
                .AddDefaultAuditSink();
```

### Usage in code

```
            // Create fake product
            var productDto = new ProductDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Category = Guid.NewGuid().ToString()
            };

            // Log this action
            var productGetUserEvent = new ProductGetEvent
            {
                Category = nameof(ProductGetEvent),
                Product = productDto
            };

            var productGetMachineEvent = new ProductGetEvent
            {
                Category = nameof(ProductGetEvent),
                Product = productDto,
                SubjectType = AuditSubjectTypes.Machine,
                SubjectName = Environment.MachineName,
                SubjectIdentifier = Environment.MachineName,
                Action = new { Method = nameof(Get), Class = nameof(AuditController) }
            };

            await _auditEventLogger.LogEventAsync(productGetMachineEvent, options =>
                {
                    options.UseDefaultSubject = false;
                    options.UseDefaultAction = false;
                });

            await _auditEventLogger.LogEventAsync(productGetUserEvent);
```

**ProductAddedEvent.cs**
```
public class ProductAddedEvent : AuditEvent
    {
        public ProductDto ProductDto { get; set; }  
    }
```

## Setup default IAuditSubject and IAuditAction

**Default action implementation:**
```
public class AuditHttpAction : IAuditAction
    {
        public AuditHttpAction(IHttpContextAccessor accessor)
        {
            Action = new
            {
                TraceIdentifier = accessor.HttpContext.TraceIdentifier,
                RequestUrl = accessor.HttpContext.Request.GetDisplayUrl(),
                HttpMethod = accessor.HttpContext.Request.Method,
                FormVariables = HttpContextHelpers.GetFormVariables(accessor.HttpContext)
            };
        }

        public object Action { get; set; }
    }
```

**Default subject implementation:**

```
public class AuditHttpSubject : IAuditSubject
    {
        public AuditHttpSubject(IHttpContextAccessor accessor)
        {
            SubjectIdentifier = accessor.HttpContext.User.FindFirst(ClaimsConsts.Sub)?.Value;
            SubjectName = accessor.HttpContext.User.FindFirst(ClaimsConsts.Name)?.Value;
            SubjectAdditionalData = new
            {
                RemoteIpAddress = accessor.HttpContext.Connection?.RemoteIpAddress?.ToString(),
                Claims = accessor.HttpContext.User.Claims?.ToDictionary(t => t.Type, t => t.Value)
            };
        }

        public string SubjectName { get; set; }

        public string SubjectType { get; set; } = AuditSubjectTypes.User;

        public object SubjectAdditionalData { get; set; }

        public string SubjectIdentifier { get; set; }
    }
```

# Example
- Please, check out the project Skoruba.AuditLogging.Host - which contains example with Asp.Net Core API - with fake authentication for testing purpose only. 😊

# Licence
This repository is licensed under the terms of the MIT license.

**NOTE:** This repository uses the source code from https://github.com/IdentityServer/IdentityServer4 which is under the terms of the **Apache License 2.0**.

# Acknowledgements

- This library is inspired by [EventService from IdentityServer4](https://github.com/IdentityServer/IdentityServer4).
