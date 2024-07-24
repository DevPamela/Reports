using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Service
{
    public interface IBulkInsertHelper
    {
        Task BulkInsert<T>(IEnumerable<T> items, string tableName, string[] columnNames);
    }
}
