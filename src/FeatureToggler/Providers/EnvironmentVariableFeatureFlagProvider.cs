namespace FeatureToggler.Providers;

/// <summary>
/// Provides feature flags from environment variables.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EnvironmentVariableFeatureFlagProvider"/> class.
/// </remarks>
/// <param name="mappings">A dictionary mapping feature flag keys to environment variable names.</param>
public class EnvironmentVariableFeatureFlagProvider(Dictionary<string, string> mappings) : IFeatureFlagProvider
{
    private readonly Dictionary<string, string> _mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));

    /// <inheritdoc />
    public Task<IEnumerable<FeatureFlag>> LoadFlagsAsync()
    {
        var flags = new List<FeatureFlag>();

        foreach (KeyValuePair<string, string> mapping in _mappings)
        {
            string? value = Environment.GetEnvironmentVariable(mapping.Value);
            if (bool.TryParse(value, out bool isEnabled))
            {
                flags.Add(new FeatureFlag(mapping.Key, isEnabled));
            }
        }

        return Task.FromResult(flags.AsEnumerable());
    }
}
