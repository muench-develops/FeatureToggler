using Microsoft.Extensions.Configuration;

namespace FeatureToggler.Providers;

/// <summary>
/// Loads feature flags from IConfiguration, typically appsettings.json or other configuration sources.
/// </summary>
public class ConfigurationFeatureFlagProvider(IConfiguration configuration) : IFeatureFlagProvider
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    /// <inheritdoc />
    public async Task<IEnumerable<FeatureFlag>> LoadFlagsAsync()
    {
        IConfigurationSection featureToggles = _configuration.GetSection("FeatureToggles");
        var flags = new List<FeatureFlag>();

        foreach (IConfigurationSection feature in featureToggles.GetChildren())
        {
            string isEnabledString = feature.GetSection("IsEnabled")?.Value ?? "false";
            string description = feature.GetSection("Description")?.Value ?? string.Empty;

            bool isEnabled = bool.TryParse(isEnabledString, out bool result) && result;

            flags.Add(new FeatureFlag(feature.Key, isEnabled, description));
        }

        return await Task.FromResult(flags);
    }

}
