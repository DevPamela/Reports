namespace Reports.Application.Settings;

internal class ReportsDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string StudentsCollectionName { get; set; } = string.Empty;
    public string GoalsCollectionName { get; set; } = string.Empty;
    public bool IsSSL { get; set; }

}
