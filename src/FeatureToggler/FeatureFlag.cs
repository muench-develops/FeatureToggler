namespace FeatureToggler;

/// <summary>
/// Represents a single feature flag with its state, description, and unique key.
/// </summary>
public class FeatureFlag
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureFlag"/> class.
    /// </summary>
    /// <param name="key">The unique identifier for the feature flag.</param>
    /// <param name="isEnabled">The initial state of the feature flag.</param>
    /// <param name="description">Optional description for the feature flag.</param>
    public FeatureFlag(string key, bool isEnabled, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));
        }

        Key = key;
        IsEnabled = isEnabled;
        Description = description;
    }

    /// <summary>
    /// Gets the unique identifier for the feature flag.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets a value indicating whether the feature is enabled.
    /// </summary>
    public bool IsEnabled { get; private set; }

    /// <summary>
    /// Gets the optional description of the feature flag.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Updates the state of the feature flag.
    /// </summary>
    /// <param name="isEnabled">The new state of the feature flag.</param>
    public void UpdateState(bool isEnabled) => IsEnabled = isEnabled;
}
