using AngleSharp.Io.Dom;

namespace Reports.Tests.TestHelpers;

/// <summary>
/// Represents a file entry.
/// </summary>
public class FileEntry : IFile
{
    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Type { get; }

    /// <inheritdoc />
    public Stream Body { get; }

    /// <inheritdoc />
    public DateTime LastModified { get; }

    /// <inheritdoc />
    public bool IsClosed => !Body.CanRead;

    /// <inheritdoc />
    public int Length => (int)Body.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileEntry"/> class.
    /// </summary>
    /// <param name="name">The name of the file.</param>
    /// <param name="type">The type of the file.</param>
    /// <param name="body">The body of the file.</param>
    public FileEntry(string name, string type, Stream body)
    {
        Name = name;
        Type = type;
        Body = body;
        LastModified = DateTime.Now;
    }

    /// <inheritdoc />
    public IBlob Slice(int start = 0, int end = int.MaxValue, string? contentType = null)
    {
        var ms = new MemoryStream();
        var buffer = new byte[Math.Max(0, Math.Min(end, Body.Length) - start)];

        Body.Position = start;
        var bytesRead = Body.Read(buffer, 0, buffer.Length);

        ms.Write(buffer, 0, Math.Max(buffer.Length, bytesRead));
        Body.Position = 0;

        return new FileEntry(Name, contentType ?? Type, ms);
    }

    /// <inheritdoc />
    public void Close()
    {
        Body.Close();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Body.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
