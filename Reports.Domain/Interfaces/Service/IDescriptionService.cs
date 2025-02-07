using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Service
{
    public interface IDescriptionService
    {
        Task AddDescription(string description, string collectionName);
    }
}
