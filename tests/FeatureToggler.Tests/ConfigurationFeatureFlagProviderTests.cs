using FeatureToggler.Providers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FeatureToggler.Tests;

public class ConfigurationFeatureFlagProviderTests
{
    [Fact]
    public async Task LoadFlagsAsyncShouldLoadFlagsCorrectlyWhenConfigurationIsValid()
    {
        // Arrange: Mocking IConfiguration for valid feature toggles
        var mockConfiguration = new Mock<IConfiguration>();
        var mockFeatureSection = new Mock<IConfigurationSection>();
        var mockFeatureChildren = new List<IConfigurationSection>
        {
            CreateMockSection("EnableNewUI", "true", "Enables the new UI").Object,
            CreateMockSection("BetaFeature", "false", "Beta feature toggle").Object
        };

        mockFeatureSection.Setup(s => s.GetChildren()).Returns(mockFeatureChildren);
        mockConfiguration.Setup(c => c.GetSection("FeatureToggles")).Returns(mockFeatureSection.Object);

        var provider = new ConfigurationFeatureFlagProvider(mockConfiguration.Object);

        // Act
        IEnumerable<FeatureFlag> flags = await provider.LoadFlagsAsync();

        // Assert
        flags.Should().HaveCount(2);

        FeatureFlag enableNewUIFlag = flags.Should().ContainSingle(f => f.Key == "EnableNewUI").Subject;
        enableNewUIFlag.IsEnabled.Should().BeTrue();
        enableNewUIFlag.Description.Should().Be("Enables the new UI");

        FeatureFlag betaFeatureFlag = flags.Should().ContainSingle(f => f.Key == "BetaFeature").Subject;
        betaFeatureFlag.IsEnabled.Should().BeFalse();
        betaFeatureFlag.Description.Should().Be("Beta feature toggle");
    }

    [Fact]
    public async Task LoadFlagsAsyncShouldHandleMissingIsEnabledGracefully()
    {
        // Arrange: Mocking IConfiguration for missing "IsEnabled"
        var mockConfiguration = new Mock<IConfiguration>();
        var mockFeatureSection = new Mock<IConfigurationSection>();
        var mockFeatureChildren = new List<IConfigurationSection>
        {
            CreateMockSection("EnableNewUI", string.Empty, "Enables the new UI").Object
        };

        mockFeatureSection.Setup(s => s.GetChildren()).Returns(mockFeatureChildren);
        mockConfiguration.Setup(c => c.GetSection("FeatureToggles")).Returns(mockFeatureSection.Object);

        var provider = new ConfigurationFeatureFlagProvider(mockConfiguration.Object);

        // Act
        IEnumerable<FeatureFlag> flags = await provider.LoadFlagsAsync();

        // Assert
        flags.Should().NotBeNull();
        flags.Should().HaveCount(1);
        FeatureFlag flag = flags.First();
        flag.Key.Should().Be("EnableNewUI");
        flag.IsEnabled.Should().BeFalse(); // Default value when "IsEnabled" is missing
        flag.Description.Should().Be("Enables the new UI");
    }

    [Fact]
    public async Task LoadFlagsAsyncShouldHandleMissingDescriptionGracefully()
    {
        // Arrange: Mocking IConfiguration for missing "Description"
        var mockConfiguration = new Mock<IConfiguration>();
        var mockFeatureSection = new Mock<IConfigurationSection>();
        var mockFeatureChildren = new List<IConfigurationSection>
        {
            CreateMockSection("EnableNewUI", "true", string.Empty).Object
        };

        mockFeatureSection.Setup(s => s.GetChildren()).Returns(mockFeatureChildren);
        mockConfiguration.Setup(c => c.GetSection("FeatureToggles")).Returns(mockFeatureSection.Object);

        var provider = new ConfigurationFeatureFlagProvider(mockConfiguration.Object);

        // Act
        IEnumerable<FeatureFlag> flags = await provider.LoadFlagsAsync();

        // Assert
        flags.Should().HaveCount(1);

        FeatureFlag enableNewUIFlag = flags.Should().ContainSingle(f => f.Key == "EnableNewUI").Subject;
        enableNewUIFlag.IsEnabled.Should().BeTrue();
        enableNewUIFlag.Description.Should().BeEmpty(); // Default value
    }

    [Fact]
    public async Task LoadFlagsAsyncShouldReturnEmptyListWhenFeatureTogglesAreMissing()
    {
        // Arrange: Mocking IConfiguration for empty feature toggles
        var mockConfiguration = new Mock<IConfiguration>();
        var mockFeatureSection = new Mock<IConfigurationSection>();

        mockFeatureSection.Setup(s => s.GetChildren()).Returns([]);
        mockConfiguration.Setup(c => c.GetSection("FeatureToggles")).Returns(mockFeatureSection.Object);

        var provider = new ConfigurationFeatureFlagProvider(mockConfiguration.Object);

        // Act
        IEnumerable<FeatureFlag> flags = await provider.LoadFlagsAsync();

        // Assert
        flags.Should().BeEmpty();
    }

    private static Mock<IConfigurationSection> CreateMockSection(string key, string isEnabled, string description)
    {
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(s => s.Key).Returns(key);

        // Mock IsEnabled and Description sub-sections
        var isEnabledSection = new Mock<IConfigurationSection>();
        isEnabledSection.Setup(s => s.Value).Returns(isEnabled);

        var descriptionSection = new Mock<IConfigurationSection>();
        descriptionSection.Setup(s => s.Value).Returns(description);

        mockSection.Setup(s => s.GetSection("IsEnabled")).Returns(isEnabledSection.Object);
        mockSection.Setup(s => s.GetSection("Description")).Returns(descriptionSection.Object);

        return mockSection;
    }
}
