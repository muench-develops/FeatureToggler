using FeatureToggler.Providers;

namespace FeatureToggler;

/// <summary>
/// Factory for creating instances of FeatureFlagManager.
/// </summary>
public static class FeatureFlagManagerFactory
{
    /// <summary>
    /// Creates an instance of FeatureFlagManager from a list of providers.
    /// </summary>
    public static FeatureFlagManager Create(IEnumerable<IFeatureFlagProvider> providers)
    {
        List<IFeatureFlagProvider> providerList = providers?.ToList() ?? [];
        if (providerList.Count == 0)
        {
            throw new ArgumentException("At least one provider must be supplied.", nameof(providers));
        }

        var flags = providerList
            .SelectMany(provider => provider.LoadFlagsAsync().Result)
            .ToList();

        return new DerivedFeatureFlagManager(flags);
    }

    /// <summary>
    /// Derived class to expose the protected constructor.
    /// </summary>
    private sealed class DerivedFeatureFlagManager(IEnumerable<FeatureFlag> flags) : FeatureFlagManager(flags)
    {
    }
}
