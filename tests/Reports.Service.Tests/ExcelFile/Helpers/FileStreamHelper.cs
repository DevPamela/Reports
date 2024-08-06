namespace Reports.Service.Tests.ExcelFile.Helpers;
public class FileStreamHelper : IDisposable
{
    public FileStream FileStream { get; }

    public FileStreamHelper(params string[] paths)
    {
        FileStream = File.OpenRead(Path.Combine(
            Directory.GetCurrentDirectory(), "ExcelFile", "Files", $"{Path.Combine(paths)}.xlsx"));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            FileStream?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
