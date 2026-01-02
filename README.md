![Logo](docs/Images/logo.png) 

# TechAdvisor.AuditLogging
> Simple audit logging for .NET Core with EntityFramework Core support

# How to install

```ps
dotnet add package TechAdvisor.AuditLogging.EntityFramework --version 8.0.0
```

# How to use it

## Setup for web application and auditing of users:

```csharp
services.AddAuditLogging(options =>
                {
                    options.Enabled = true;
                    options.UseDefaultSubject = true;
                    options.UseDefaultAction = true;
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
```

## Setup for machine application (e.g. background jobs):
```csharp
services.AddAuditLogging(options =>
                {
                    options.UseDefaultAction = false;
                    options.Source = "Web"
                })
                .AddStaticEventSubject(subject =>
                {
                    subject.SubjectType = AuditSubjectTypes.Machine;
                    subject.SubjectIdentifier = EmailServiceConsts.Name;
                    subject.SubjectName = Environment.MachineName;
                })
                .AddDefaultEventAction()
                .AddStore<ApplicationDbContext, AuditLog, AuditLoggingRepository<ApplicationDbContext, AuditLog>>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection"),
                        optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
                .AddDefaultAuditSink();
```


## Usage in code

```csharp
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
                Product = productDto
            };           
```

### Logging for user action
```csharp
 await _auditEventLogger.LogEventAsync(productGetUserEvent);
```

### Logging for machine action
```csharp
var productGetMachineEvent = new ProductGetEvent
            {
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
```

**ProductAddedEvent.cs**
```csharp
public class ProductAddedEvent : AuditEvent
    {
        public ProductDto ProductDto { get; set; }  
    }
```

## AuditEvent - base event for logger

| Property              | Description                                             |
|-----------------------|---------------------------------------------------------|
| Event                 | Name of event                                           |
| Source                | Source of logging events                                |
| Category              | Event category                                          |
| SubjectIdentifier     | Identifier of caller which is responsible for the event |
| SubjectName           | Name of caller which is responsible for the event       |
| SubjectType           | Subject Type (eg. User/Machine)                         |
| SubjectAdditionalData | Additional information for subject                      |
| Action                | Information about request/action                        |

## AuditLog - database table

| Property              | Description                                             |
|-----------------------|---------------------------------------------------------|
| Id                    | Database unique identifier for event                    |
| Event                 | Name of event                                           |
| Source                | Source of logging events                                |
| Category              | Event category                                          |
| SubjectIdentifier     | Identifier of caller which is responsible for the event |
| SubjectName           | Name of caller which is responsible for the event       |
| SubjectType           | Subject Type (eg. User/Machine)                         |
| SubjectAdditionalData | Additional information for subject                      |
| Action                | Information about request/action                        |
| Data                  | Data which are serialized into JSON format              |
| Created               | Date and time for creating of the event                 |

## Setup default IAuditSubject and IAuditAction

## `IAuditSubject`
**Default subject implementation for HTTP calls:**

```csharp
public class AuditHttpSubject : IAuditSubject
    {
        public AuditHttpSubject(IHttpContextAccessor accessor, AuditHttpSubjectOptions options)
        {
            SubjectIdentifier = accessor.HttpContext.User.FindFirst(options.SubjectIdentifierClaim)?.Value;
            SubjectName = accessor.HttpContext.User.FindFirst(options.SubjectNameClaim)?.Value;
            SubjectAdditionalData = new
            {
                RemoteIpAddress = accessor.HttpContext.Connection?.RemoteIpAddress?.ToString(),
                LocalIpAddress = accessor.HttpContext.Connection?.LocalIpAddress?.ToString(),
                Claims = accessor.HttpContext.User.Claims?.Select(x=> new { x.Type, x.Value })
            };
        }

        public string SubjectName { get; set; }

        public string SubjectType { get; set; } = AuditSubjectTypes.User;

        public object SubjectAdditionalData { get; set; }

        public string SubjectIdentifier { get; set; }
    }
```

## `IAuditAction`
**Default action implementation for HTTP calls:**
```csharp
public class AuditHttpAction : IAuditAction
    {
        public AuditHttpAction(IHttpContextAccessor accessor, AuditHttpActionOptions options)
        {
            Action = new
            {
                TraceIdentifier = accessor.HttpContext.TraceIdentifier,
                RequestUrl = accessor.HttpContext.Request.GetDisplayUrl(),
                HttpMethod = accessor.HttpContext.Request.Method,
                FormVariables = options.IncludeFormVariables ? HttpContextHelpers.GetFormVariables(accessor.HttpContext) : null
            };
        }

        public object Action { get; set; }
    }
```

## Sinks

### Database migrations

```ps
dotnet ef migrations add DbInit -c DefaultAuditLoggingDbContext -o Data/Migrations
dotnet ef database update -c DefaultAuditLoggingDbContext
```

### Database sink via EntityFramework Core - `DatabaseAuditEventLoggerSink`

- By default it is used database sink via EntityFramework Core, for registration this default sink - it is required to register this method:

```csharp
.AddDefaultStore(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext"),
                    optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)))
                .AddDefaultAuditSink()
```

### AddDefaultStore:

- This method register default implementation of:

- `DefaultAuditLoggingDbContext` - Default DbContext for access to database
- `AuditLog` - Entity for logging all audit stuff
- `AuditLoggingRepository` - Repository for access to database, which contains GRUD method for access to `AuditLog` table.

- In the background it is used method called: `AddStore` - which is possible to use instead of AddDefaultStore and specify individual implementation of these objects above
```csharp
builder.AddStore<DefaultAuditLoggingDbContext, AuditLog, AuditLoggingRepository<DefaultAuditLoggingDbContext, AuditLog>>(dbContextOptions);
```

### AddDefaultAuditSink:

- This method is for registration of default Sink:

```csharp
builder.AddAuditSinks<DatabaseAuditEventLoggerSink<AuditLog>>();
```

# How to use own Sink

- It is necessary to implement interface `IAuditEventLoggerSink` and one single method called:

```csharp
Task PersistAsync(AuditEvent auditEvent);
```

- Then you can register your new sink via method - `.AddAuditSinks<>` - which has overload for maximum 8 sinks.

# Example

## Source code
- Please, check out the project `TechAdvisor.AuditLogging.Host` - which contains example with Asp.Net Core API - with fake authentication for testing purpose only. 😊

## Output in JSON format

```json
{
      "Id":1,
      "Event":"ProductGetEvent",
      "Category":"Web",
      "SubjectIdentifier":"30256997-4096-428d-bfc7-8593d263b8eb",
      "SubjectName":"bob",
      "SubjectType":"User",
      "SubjectAdditionalData":{
         "RemoteIpAddress":"::1",
         "LocalIpAddress":"::1",
         "Claims":[
            {
               "Type":"name",
               "Value":"bob"
            },
            {
               "Type":"sub",
               "Value":"30256997-4096-428d-bfc7-8593d263b8eb"
            },
            {
               "Type":"role",
               "Value":"31fad6ad-9df3-4e7f-b73f-68dc7d2636c6"
            }
         ]
      },
      "Action":{
         "TraceIdentifier":"80000025-0000-ff00-b63f-84710c7967bb",
         "RequestUrl":"https://localhost:44319/api/audit",
         "HttpMethod":"GET"
      },
      "Data":{
         "Product":{
            "Id":"7d7138b6-e5c3-4548-814c-9119ddb1f785",
            "Name":"c9bc91fe-79f2-439b-8bfa-be3f71947b63",
            "Category":"b3f2f9d2-67d5-4b52-8156-04232adf0c4b"
         }
      },
      "Created":"2019-09-09T12:03:12.7729634"
   }
```

# Licence
This repository is licensed under the terms of the MIT license.

**NOTE:** This repository uses the source code from https://github.com/IdentityServer/IdentityServer4 which is under the terms of the **[Apache License 2.0](https://github.com/IdentityServer/IdentityServer4/blob/master/LICENSE)**.

# Acknowledgements

- This library is inspired by [EventService from IdentityServer4](https://github.com/IdentityServer/IdentityServer4).
