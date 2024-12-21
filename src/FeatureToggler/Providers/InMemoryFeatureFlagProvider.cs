namespace FeatureToggler.Providers;

/// <summary>
/// Provides feature flags from an in-memory collection.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InMemoryFeatureFlagProvider"/> class.
/// </remarks>
/// <param name="flags">The initial collection of feature flags.</param>
public class InMemoryFeatureFlagProvider(IEnumerable<FeatureFlag> flags) : IFeatureFlagProvider
{
    private readonly IEnumerable<FeatureFlag> _flags = flags ?? throw new ArgumentNullException(nameof(flags));

    /// <inheritdoc />
    public Task<IEnumerable<FeatureFlag>> LoadFlagsAsync() => Task.FromResult(_flags);
}
