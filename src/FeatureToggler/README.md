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
#### In-Memory Provider
For quick development and testing:
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
#### JSON Provider
To manage feature flags via a JSON file, use `JsonFeatureFlagProvider`:

1. Create a JSON file, e.g., `featureflags.json`:
```json
[
    { "Key": "EnableNewUI", "IsEnabled": true, "Description": "Enables the new UI" },
    { "Key": "BetaFeature", "IsEnabled": false, "Description": "Beta feature toggle" }
]
```
2. Register the provider in `Program.cs`:

```csharp
builder.Services.AddFeatureFlags(
    new JsonFeatureFlagProvider("path/to/featureflags.json", new FileReader())
);
```

#### Environment Variable Provider
Use environment variables to control feature flags dynamically:

1. Set environment variables:
```bash
$env:ENABLE_NEW_UI = "true"
$env:BETA_FEATURE = "false"
```
2. Register the provider in Program.cs:

```csharp
builder.Services.AddFeatureFlags(
    new EnvironmentVariableFeatureFlagProvider(new Dictionary<string, string>
    {
        { "EnableNewUI", "ENABLE_NEW_UI" },
        { "BetaFeature", "BETA_FEATURE" }
    })
);
```
#### Configuration Provider
The `ConfigurationFeatureFlagProvider` enables you to manage feature toggles directly from the `.NET IConfiguration` system, which can include sources like `appsettings.json`, environment variables, or other configuration providers.

```json
{
  "FeatureToggles": {
    "EnableNewUI": {
      "IsEnabled": true,
      "Description": "Enables the new UI"
    },
    "BetaFeature": {
      "IsEnabled": false,
      "Description": "This is a beta feature toggle"
    }
  }
}

```

```csharp
using FeatureToggler.Providers;

var builder = WebApplication.CreateBuilder(args);

// Register Feature Flags with the Configuration Provider
builder.Services.AddFeatureFlags(
    new ConfigurationFeatureFlagProvider(builder.Configuration)
);

var app = builder.Build();
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
