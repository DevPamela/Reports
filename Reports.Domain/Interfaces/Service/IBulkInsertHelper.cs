namespace Reports.Domain.Interfaces.Service
{
    public interface IBulkInsertHelper
    {
        Task BulkInsert<T>(IEnumerable<T> items, string tableName, string[] columnNames);
    }
}
