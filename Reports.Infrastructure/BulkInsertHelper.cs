using MongoDB.Driver;
using MySqlConnector;
using Reports.Domain.Interfaces.Service;
using System.Data;
using System.Reflection;
using System.Text;

public class BulkInsertHelper : IBulkInsertHelper
{
    public async Task BulkInsert<T>(IEnumerable<T> items, string tableName, string[] columnNames)
    {
        try
        {
            if (items == null || !items.Any() || columnNames == null || columnNames.Length == 0) return;
            string connectionString = Environment.GetEnvironmentVariable("ADMIN_CONNECTION_STRING");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length < columnNames.Length) throw new ArgumentException("More column names than properties in the entity.");

            StringBuilder sCommand = new StringBuilder($"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ");

            using (MySqlConnection mConnection = new MySqlConnection(connectionString))
            {
                List<string> rows = new List<string>();

                foreach (var item in items)
                {
                    var values = new List<string>();
                    foreach (var columnName in columnNames)
                    {
                        var property = properties.FirstOrDefault(p => string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));
                        if (property != null)
                        {
                            var value = property.GetValue(item, null);
                            values.Add(ConvertToSqlValue(value));
                        }
                        else
                        {
                            values.Add("NULL");
                        }
                    }
                    rows.Add($"({string.Join(", ", values)})");
                }

                sCommand.Append(string.Join(",", rows));
                sCommand.Append(";");

                mConnection.Open();

                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    await myCmd.ExecuteNonQueryAsync();
                }
            }
        }

        catch (Exception e)
        {
            new Exception(e.Message);
        }
    }
    private string ConvertToSqlValue(object value)
    {
        if (value == null)
        {
            return "NULL";
        }

        return value switch
        {
            DateTime dateTime => $"'{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}'",
            bool boolValue => boolValue ? "1" : "0",
            string stringValue => $"'{MySqlHelper.EscapeString(stringValue)}'",
            int intValue => intValue.ToString(),
            _ => $"'{MySqlHelper.EscapeString(value.ToString())}'"
        };
    }
}
