using FeatureToggler;
using FeatureToggler.Extensions;
using FeatureToggler.Providers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register Feature Flags
builder.Services.AddFeatureFlags(
    // new InMemoryFeatureFlagProvider(
    // [
    //     new FeatureFlag("EnableNewUI", true, "Enables the new UI"),
    //     new FeatureFlag("BetaFeature", false, "Beta feature toggle")
    // ]),
    // new JsonFeatureFlagProvider("featureflags.json")
    new EnvironmentVariableFeatureFlagProvider(new Dictionary<string, string>
    {
        { "EnableNewUI", "ENABLE_NEW_UI" },
        { "BetaFeature", "BETA_FEATURE" }
    })
);


WebApplication app = builder.Build();

// Use Middleware for a specific feature flag
app.UseFeatureFlag("EnableNewUI",
    onEnabled: () => Console.WriteLine("New UI is enabled!"),
    onDisabled: () => Console.WriteLine("New UI is disabled!"));

// Use Middleware for a specific feature flag
app.UseFeatureFlag("BetaFeature",
    onEnabled: () => Console.WriteLine("BetaFeature is enabled!"),
    onDisabled: () => Console.WriteLine("BetaFeature is disabled!"));


// Example route for "EnableNewUI"
app.MapGet("/", (FeatureFlagManager manager) => manager.IsEnabled("EnableNewUI")
        ? Results.Ok("Welcome to the new UI!")
        : Results.Ok("Welcome to the old UI!"));

// Example route for "BetaFeature" when disabled
app.MapGet("/beta", (FeatureFlagManager manager) => !manager.IsEnabled("BetaFeature")
        ? Results.Ok("Beta feature is disabled. Access restricted.")
        : Results.Ok("Beta feature is enabled. Welcome to the beta program!"));

await app.RunAsync();