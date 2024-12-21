namespace FeatureToggler.Providers;

/// <summary>
/// Interface for loading feature flags from various sources.
/// </summary>
public interface IFeatureFlagProvider
{
    /// <summary>
    /// Loads a collection of feature flags.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="FeatureFlag"/>.</returns>
    Task<IEnumerable<FeatureFlag>> LoadFlagsAsync();
}
