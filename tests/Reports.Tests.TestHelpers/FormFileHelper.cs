using Microsoft.AspNetCore.Http;

namespace Reports.Tests.TestHelpers;

/// <summary>
/// Helper class for working with <see cref="IFormFile"/> objects.
/// </summary>
public class FormFileHelper : IDisposable
{
    private readonly MemoryStream _memoryStream = new();

    /// <summary>
    /// Gets the <see cref="IFormFile"/> object.
    /// </summary>
    public IFormFile FormFile { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormFileHelper"/> class.
    /// </summary>
    /// <param name="fileName">The file name of the form file.</param>
    /// <param name="length">The length of the form file.</param>
    public FormFileHelper(string fileName, long length = 0)
    {
        FormFile = new FormFile(_memoryStream, 0, length, string.Empty, fileName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormFileHelper"/> class.
    /// </summary>
    /// <param name="stream">The stream containing the form file data.</param>
    /// <param name="fileName">The file name of the form file.</param>
    public FormFileHelper(Stream stream, string fileName)
    {
        FormFile = new FormFile(stream, 0, stream.Length, string.Empty, fileName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _memoryStream.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
