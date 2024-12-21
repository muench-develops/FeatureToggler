namespace FeatureToggler.Utils;

public class FileReader : IFileReader
{
    public async Task<string> ReadFileAsync(string path) => await File.ReadAllTextAsync(path);
}
