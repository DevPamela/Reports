namespace Reports.Tests.TestHelpers.Files;

/// <summary>
/// Helper class for working with <see cref="System.IO.FileStream"/>.
/// </summary>
public class FileStreamHelper : IDisposable
{
    /// <summary>
    /// Gets the file stream.
    /// </summary>
    public FileStream FileStream { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStreamHelper"/> class.
    /// </summary>
    /// <param name="paths">The path fragments of the file to open.</param>
    public FileStreamHelper(params string[] paths)
    {
        FileStream = File.OpenRead(Path.Combine(
            Directory.GetCurrentDirectory(), $"{Path.Combine(paths)}"));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            FileStream.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
