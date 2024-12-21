using System.Text.Json;
using FeatureToggler.Utils;

namespace FeatureToggler.Providers;

/// <summary>
/// Provides feature flags from a JSON file.
/// </summary>
public class JsonFeatureFlagProvider : IFeatureFlagProvider
{
    private readonly string _filePath;
    private readonly IFileReader _fileReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFeatureFlagProvider"/> class.
    /// </summary>
    /// <param name="filePath">Path to the JSON file containing feature flags.</param>
    /// <param name="fileReader">File reader abstraction for reading the file.</param>
    public JsonFeatureFlagProvider(string filePath, IFileReader fileReader)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        _filePath = filePath;
        _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FeatureFlag>> LoadFlagsAsync()
    {
        string json = await _fileReader.ReadFileAsync(_filePath);

        // Check if JSON is null or empty
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<FeatureFlag>>(json) ?? [];
    }
}