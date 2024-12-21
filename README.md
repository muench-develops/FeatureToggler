# muench-develops.FeatureToggler


[![NuGet](https://img.shields.io/nuget/v/muench-develops.FeatureToggler.svg)](https://www.nuget.org/packages/muench-develops.FeatureToggler/) [![NuGet Downloads](https://img.shields.io/nuget/dt/muench-develops.FeatureToggler.svg)](https://www.nuget.org/packages/muench-develops.FeatureToggler/)

`muench-develops.FeatureToggler`  is a lightweight, extensible library for managing feature flags in .NET applications. It supports multiple providers (in-memory, JSON, environment variables, databases) and integrates seamlessly with ASP.NET Core.

## Features

- **Multiple Providers**: In-Memory, JSON, Environment Variables, Database (not yet).
- **ASP.NET Core Integration**: Middleware and Dependency Injection support.
- **Thread-Safe**: Concurrent updates handled efficiently.
- **Testable**: Clean design for easy unit testing.
- **Extensible**: Add custom providers effortlessly.

## Installation

Install FeatureToggler via NuGet:

```bash
dotnet add package muench-develops.FeatureToggler
```

## Getting Started
####  1. Register Feature Flags
Add providers in your Program.cs or Startup.cs:

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
app.MapGet("/", (FeatureFlagManager manager) => manager.IsEnabled("EnableNewUI")
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

## Available Providers
- In-Memory: Simple and fast for development.
- JSON: Load feature flags from a JSON file.
- Environment Variables: Configure flags via environment variables.

## Contributing
We welcome contributions! Please fork the repository and submit a pull request with your changes. For major updates, open an issue first to discuss your ideas.

## License
This project is licensed under the Apache License. See the [LICENSE](LICENSE) file for details.