using Microsoft.AspNetCore.Http;

namespace Reports.Domain.Interfaces.Service
{
    public interface ILocalStorageService
    {
         Task<string> SaveFile(IFormFile file);
    }
}
