using FeatureToggler.Providers;
using FluentAssertions;

namespace FeatureToggler.Tests;

/// <summary>
/// Unit tests for EnvironmentVariableFeatureFlagProvider.
/// </summary>
public class EnvironmentVariableFeatureFlagProviderTests
{
    [Fact]
    public async Task LoadFlagsAsyncShouldLoadFlagsFromEnvironmentVariables()
    {
        // Arrange
        Environment.SetEnvironmentVariable("FEATURE_A", "true");
        Environment.SetEnvironmentVariable("FEATURE_B", "false");

        var provider = new EnvironmentVariableFeatureFlagProvider(new Dictionary<string, string>
        {
            { "FeatureA", "FEATURE_A" },
            { "FeatureB", "FEATURE_B" }
        });

        // Act
        var flags = (await provider.LoadFlagsAsync()).ToList();

        // Assert
        flags.Should().HaveCount(2);
        flags[0].Key.Should().Be("FeatureA");
        flags[0].IsEnabled.Should().BeTrue();

        flags[1].Key.Should().Be("FeatureB");
        flags[1].IsEnabled.Should().BeFalse();

        // Cleanup
        Environment.SetEnvironmentVariable("FEATURE_A", null);
        Environment.SetEnvironmentVariable("FEATURE_B", null);
    }

    [Fact]
    public async Task LoadFlagsAsyncShouldIgnoreUnsetEnvironmentVariables()
    {
        // Arrange
        var provider = new EnvironmentVariableFeatureFlagProvider(new Dictionary<string, string>
        {
            { "FeatureA", "FEATURE_A" }
        });

        // Act
        IEnumerable<FeatureFlag> flags = await provider.LoadFlagsAsync();

        // Assert
        flags.Should().BeEmpty();
    }
}
