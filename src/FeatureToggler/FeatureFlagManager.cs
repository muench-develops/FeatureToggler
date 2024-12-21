using System.Collections.Concurrent;

namespace FeatureToggler;

/// <summary>
/// Manages feature flags and provides methods to query or update their states.
/// </summary>
public class FeatureFlagManager
{
    private readonly ConcurrentDictionary<string, FeatureFlag> _flags = new();

    /// <summary>
    /// Protected constructor for controlled instantiation.
    /// </summary>
    protected FeatureFlagManager(IEnumerable<FeatureFlag> flags)
    {
        foreach (FeatureFlag flag in flags)
        {
            _flags[flag.Key] = flag;
        }
    }
    /// <summary>
    /// Checks if a feature flag is enabled.
    /// </summary>
    /// <param name="key">The unique key of the feature flag.</param>
    /// <returns><c>true</c> if the flag is enabled; otherwise, <c>false</c>.</returns>
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public bool IsEnabled(string key)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CA1822 // Mark members as static
    {
        if (!_flags.TryGetValue(key, out FeatureFlag? flag))
        {
            throw new KeyNotFoundException($"Feature flag '{key}' not found.");
        }

        return flag.IsEnabled;
    }

    /// <summary>
    /// Updates the state of a feature flag.
    /// </summary>
    /// <param name="key">The unique key of the feature flag.</param>
    /// <param name="isEnabled">The new state of the feature flag.</param>
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public void UpdateFlag(string key, bool isEnabled)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore CA1822 // Mark members as static
    {
        if (!_flags.TryGetValue(key, out FeatureFlag? flag))
        {
            throw new KeyNotFoundException($"Feature flag '{key}' not found.");
        }

        flag.UpdateState(isEnabled);
    }
}
