using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using FeatureToggler.Providers;

namespace FeatureToggler.Extensions;

/// <summary>
/// Provides extension methods for integrating feature flags into ASP.NET Core applications.
/// </summary>
public static class FeatureFlagExtensions
{
    /// <summary>
    /// Registers the <see cref="FeatureFlagManager"/> with the specified providers.
    /// </summary>
    /// <param name="services">The service collection to add the manager to.</param>
    /// <param name="providers">An array of feature flag providers to load flags from.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddFeatureFlags(
        this IServiceCollection services,
        params IFeatureFlagProvider[] providers)
    {
        if (providers == null || providers.Length == 0)
        {
            throw new ArgumentException("At least one provider must be supplied.", nameof(providers));
        }

        // Create the FeatureFlagManager and register it as a singleton
        FeatureFlagManager manager = FeatureFlagManagerFactory.Create(providers);
        return services.AddSingleton<FeatureFlagManager>(manager);
    }

    /// <summary>
    /// Middleware to execute actions based on the state of a feature flag.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="key">The unique key of the feature flag to check.</param>
    /// <param name="onEnabled">The action to execute if the flag is enabled.</param>
    /// <param name="onDisabled">The action to execute if the flag is disabled.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseFeatureFlag(
        this IApplicationBuilder app,
        string key,
        Action onEnabled,
        Action? onDisabled = null)
    {
        // Retrieve the FeatureFlagManager from the service provider
        FeatureFlagManager manager = app.ApplicationServices.GetService<FeatureFlagManager>()
                      ?? throw new InvalidOperationException("FeatureFlagManager is not registered.");

        if (manager.IsEnabled(key))
        {
            onEnabled?.Invoke();
        }
        else
        {
            onDisabled?.Invoke();
        }

        return app;
    }
}
