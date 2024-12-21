namespace FeatureToggler.Utils;

/// <summary>
/// Abstraction for reading files, allowing mocking in unit tests.
/// </summary>
public interface IFileReader
{
    /// <summary>
    /// Reads the content of a file asynchronously.
    /// </summary>
    /// <param name="path">The path of the file to read.</param>
    /// <returns>The content of the file as a string.</returns>
    Task<string> ReadFileAsync(string path);
}
