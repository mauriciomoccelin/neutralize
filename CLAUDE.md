# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore, build, and test everything
dotnet restore
dotnet build --no-restore
dotnet test --no-build --verbosity normal

# Run tests for a single project
dotnet test test/Neutralize.Core.Test --no-build --verbosity normal
dotnet test test/Neutralize.EFCore.Test --no-build --verbosity normal
dotnet test test/Neutralize.Dapper.Test --no-build --verbosity normal
dotnet test test/Neutralize.Kafka.Test --no-build --verbosity normal

# Run a single test by name
dotnet test test/Neutralize.Core.Test --filter "FullyQualifiedName~TestClassName"

# Pack NuGet packages (CI does this via build/pack.ps1 on release)
dotnet pack src/Neutralize.Core/Neutralize.Core.csproj
```

## Architecture Overview

This is a .NET 6 class library framework (C# 10) published as four NuGet packages. All packages share `RootNamespace=Neutralize`.

### Package dependency graph

```
Neutralize.Core        ← standalone, no internal deps
Neutralize.EFCore      ← depends on Neutralize.Core
Neutralize.Dapper      ← depends on Neutralize.Core
Neutralize.Kafka       ← standalone (does not depend on Core)
```

### Neutralize.Core

The foundational layer. Key abstractions:

- **Domain models** — `Entity<TId>` (base entity) and `AggregateRoot<TId>` (adds a `List<Event>` for domain events). `TId` must be a `struct`.
- **Commands** — `Command<TId>`, `CommandGuid<TId>`, `CommandInt64<TId>`. Each command has a `Validate()` / `IsValid()` / `GetErrors()` lifecycle. `CommandHandler` is the abstract base for MediatR `IRequestHandler` implementations; it wraps `IUnitOfWork`, `INeutralizeBus`, `DomainNotificationHandler`, and `IMapper`.
- **Bus** — `IInMemoryBus` / `InMemoryBus` wraps MediatR `IMediator`. Use `SendCommandGuidId<TResponse>` / `SendCommandInt64Id<TResponse>` for commands, `RaiseEvent` for domain events.
- **Notifications** — `DomainNotification` / `DomainNotificationHandler` (MediatR `INotificationHandler`) collects validation and domain errors instead of throwing exceptions.
- **Application services** — `CrudAppService` (many overloads) provides generic CRUD built on `IRepository`, AutoMapper, and `IUnitOfWork`. `IQueryAppService` / `QueryAppService` for read-only queries.
- **Identity** — `IAspNetUser` / `AspNetUser` extracts claims from `IHttpContextAccessor`.

### Neutralize.EFCore

- `EfCoreRepository<TContext, TEntity, TId>` — concrete `IRepository` implementation over EF Core. Disable `AutoDetectChangesEnabled` and use `NoTracking` in the `DbContext` constructor (the README shows the canonical pattern).
- `UnitOfWork` — wraps `DbContext.SaveChangesAsync()`.
- `DbContextExtensions` — `SetDefaultColumnTypeVarchar()` helper for `OnModelCreating`.

### Neutralize.Dapper

- `IDapperRepository` / `DapperRepository` — parameter-bag pattern (`AddParameter`), then `First<T>` or `Paged<T>`. `Paged<T>` calls `QueryMultipleAsync` expecting two queries separated by `;`: count first, then rows.
- `IDapperConnectionFactory` — implement and register to supply an open `IDbConnection`.

### Neutralize.Kafka

- `KafkaConfiguration` — built via `services.AddKafka(options => { … })`. Maps topic names → `INotification` types in `Handlers` dict.
- `KafkaFactory` — creates typed `IProducer<TKey, TValue>` and `IConsumer` instances using `KafkaJsonSerializer` / `KafkaJsonDeserializer` (Newtonsoft.Json backed).
- `KafkaMonitorConsumerService` — singleton that consumes messages and dispatches to MediatR `IMediator.Publish`. Runs inside `KafkaMonitorConsumerWorker` (`IHostedService`).
- Consumer topics must have an `INotification` handler registered with MediatR; the consumer silently warns and skips messages whose deserialized type is not `INotification`.

### Testing conventions

Tests use **xUnit** with class fixtures for shared setup, **Bogus** for fake data, **Moq.AutoMock** for mocking, and **FluentAssertions** for assertions. Each test project has a `*Fixture` class registered as `ICollectionFixture`.

### CI / Publishing

`.github/workflows/pack.yml` runs `dotnet restore → build → test` on every PR and on releases. On release it additionally runs `build/pack.ps1` (PowerShell) which reads `build/configuration.json` for project paths and versions, then pushes to NuGet. Package versions are set in `build/configuration.json`, not in the `.csproj` files directly.
