# muench-develops.FeatureToggler

`muench-develops.FeatureToggler` is a lightweight and extensible library for managing feature flags in .NET applications. It allows you to dynamically enable or disable features using multiple providers.

## Features

- **Multiple Providers:** In-Memory, JSON, Environment Variables.
- **ASP.NET Core Integration:** Middleware for feature-based request handling.
- **Extensible:** Easily add custom providers.
- **Thread-Safe:** Safe for concurrent updates and queries.

## Installation

Install via NuGet:

```bash
dotnet add package muench-develops.FeatureToggler
```

## Quick Start
### 1. Register Feature Flags
```csharp
builder.Services.AddFeatureFlags(
    new InMemoryFeatureFlagProvider(
        new[]
        {
            new FeatureFlag("EnableNewUI", true, "Enables the new UI"),
            new FeatureFlag("BetaFeature", false, "Beta feature toggle")
        })
);
```
#### 2a. Use Middleware
Execute actions based on feature flag states:
```csharp
app.UseFeatureFlag("EnableNewUI",
    onEnabled: () => Console.WriteLine("New UI is enabled!"),
    onDisabled: () => Console.WriteLine("New UI is disabled!"));
```

#### 2b. Use Minimal API
```csharp
app.MapGet("/", (FeatureFlagManager manaer) => manager.IsEnabled("EnableNewUI")
        ? Results.Ok("Welcome to the new UI!")
        : Results.Ok("Welcome to the old UI!"));
```

#### 3. Query Flags
Check or update feature flags dynamically:

```csharp
var manager = app.Services.GetRequiredService<FeatureFlagManager>();

if (manager.IsEnabled("EnableNewUI"))
{
    Console.WriteLine("New UI is enabled!");
}

```
## License
This project is licensed under the Apache License. See the [LICENSE](https://github.com/muench-develops/FeatureToggler/blob/main/LICENSE) file for details.
