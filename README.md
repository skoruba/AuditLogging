![Logo](docs/Images/Skoruba-ReadMe.png) 

# 🕊️ Skoruba.AuditLogging
> Simple audit logging for .NET Core with EntityFramework Core support

# How to install

```
dotnet add package Skoruba.AuditLogging.EntityFramework --version 1.0.0-beta1
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

# Acknowledgements

- This library is inspired by [EventService from IdentityServer4](https://github.com/IdentityServer/IdentityServer4).
