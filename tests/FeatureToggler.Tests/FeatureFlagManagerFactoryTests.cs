using FeatureToggler.Providers;
using FluentAssertions;
using Moq;

namespace FeatureToggler.Tests;

/// <summary>
/// Unit tests for the FeatureFlagManagerFactory class.
/// </summary>
public class FeatureFlagManagerFactoryTests
{
    [Fact]
    public void CreateShouldReturnFeatureFlagManager()
    {
        // Arrange
        var mockProvider = new Mock<IFeatureFlagProvider>();
        mockProvider.Setup(p => p.LoadFlagsAsync()).ReturnsAsync(
        [
            new FeatureFlag("FeatureA", true),
            new FeatureFlag("FeatureB", false)
        ]);

        // Act
        FeatureFlagManager manager = FeatureFlagManagerFactory.Create([mockProvider.Object]);

        // Assert
        Assert.NotNull(manager);
        manager.IsEnabled("FeatureA").Should().BeTrue();
        manager.IsEnabled("FeatureB").Should().BeFalse();
    }
}
