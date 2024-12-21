using FeatureToggler.Providers;
using FluentAssertions;
using Moq;

namespace FeatureToggler.Tests;

public class FeatureFlagManagerTests
{
    [Fact]
    public void ConstructorShouldInitializeWithFlags()
    {
        // Arrange
        var mockProvider = new Mock<IFeatureFlagProvider>();
        mockProvider.Setup(p => p.LoadFlagsAsync()).ReturnsAsync(
        [
            new FeatureFlag("FeatureA", true),
            new FeatureFlag("FeatureB", false)
        ]).Verifiable();

        // Act
        FeatureFlagManager manager = FeatureFlagManagerFactory.Create([mockProvider.Object]);

        // Assert
        Assert.NotNull(manager);
        manager.IsEnabled("FeatureA").Should().BeTrue();
        manager.IsEnabled("FeatureB").Should().BeFalse();
    }


    [Fact]
    public void IsEnabledShouldThrowIfFlagNotFound()
    {
        // Arrange
        var mockProvider = new Mock<IFeatureFlagProvider>();
        mockProvider.Setup(p => p.LoadFlagsAsync()).ReturnsAsync(
        [
            new FeatureFlag("FeatureA", true),
        ]).Verifiable();


        FeatureFlagManager manager = FeatureFlagManagerFactory.Create([mockProvider.Object]);

        // Act
        try
        {
            _ = manager.IsEnabled("FeatureB");
        }
        catch (KeyNotFoundException ex)
        {
            // Assert
            ex.Message.Should().Be("Feature flag 'FeatureB' not found.");
        }

    }

    [Fact]
    public void UpdateFlagShouldChangeFlagState()
    {
        // Arrange
        var mockProvider = new Mock<IFeatureFlagProvider>();
        mockProvider.Setup(p => p.LoadFlagsAsync()).ReturnsAsync(
        [
            new FeatureFlag("FeatureA", true),
        ]).Verifiable();


        FeatureFlagManager manager = FeatureFlagManagerFactory.Create([mockProvider.Object]);

        // Act
        manager.UpdateFlag("FeatureA", false);

        // Assert
        manager.IsEnabled("FeatureA").Should().BeFalse();
    }
}
