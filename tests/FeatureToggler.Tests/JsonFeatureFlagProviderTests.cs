using FeatureToggler.Providers;
using FeatureToggler.Utils;
using FluentAssertions;
using Moq;

namespace FeatureToggler.Tests;

/// <summary>
/// Unit tests for JsonFeatureFlagProvider.
/// </summary>
public class JsonFeatureFlagProviderTests
{
    [Fact]
    public async Task LoadFlagsAsyncShouldLoadFlagsFromMockedFileReader()
    {
        // Arrange
        string json = """
        [
            { "Key": "FeatureA", "IsEnabled": true, "Description": "Test feature A" },
            { "Key": "FeatureB", "IsEnabled": false, "Description": "Test feature B" }
        ]
        """;

        var mockFileReader = new Mock<IFileReader>();
        mockFileReader.Setup(fr => fr.ReadFileAsync(It.IsAny<string>())).ReturnsAsync(json);

        var provider = new JsonFeatureFlagProvider("mocked.json", mockFileReader.Object);

        // Act
        var flags = (await provider.LoadFlagsAsync()).ToList();

        // Assert
        flags.Should().HaveCount(2);
        flags[0].Key.Should().Be("FeatureA");
        flags[0].IsEnabled.Should().BeTrue();
        flags[0].Description.Should().Be("Test feature A");

        flags[1].Key.Should().Be("FeatureB");
        flags[1].IsEnabled.Should().BeFalse();
        flags[1].Description.Should().Be("Test feature B");

        // Verify the file reader was called once
        mockFileReader.Verify(fr => fr.ReadFileAsync("mocked.json"), Times.Once);
    }

    [Fact]
    public async Task LoadFlagsAsyncShouldReturnEmptyListForEmptyJson()
    {
        // Arrange
        var mockFileReader = new Mock<IFileReader>();
        mockFileReader.Setup(fr => fr.ReadFileAsync(It.IsAny<string>())).ReturnsAsync("");

        var provider = new JsonFeatureFlagProvider("mocked.json", mockFileReader.Object);

        // Act
        IEnumerable<FeatureFlag> flags = await provider.LoadFlagsAsync();

        // Assert
        flags.Should().BeEmpty();
    }

    [Fact]
    public async Task LoadFlagsAsyncShouldThrowIfFileReaderThrowsException()
    {
        // Arrange
        var mockFileReader = new Mock<IFileReader>();
        mockFileReader.Setup(fr => fr.ReadFileAsync(It.IsAny<string>()))
                      .ThrowsAsync(new FileLoadException("File read error"));

        var provider = new JsonFeatureFlagProvider("mocked.json", mockFileReader.Object);

        // Act
        Func<Task> act = provider.LoadFlagsAsync;

        // Assert
        await act.Should().ThrowAsync<FileLoadException>().WithMessage("File read error");
    }
}
