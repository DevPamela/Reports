using Microsoft.AspNetCore.Http;
using Reports.Domain.Interfaces.Service;

namespace Reports.Service
{
    public class LocalStorageService: ILocalStorageService
    {
        public async Task<string>SaveFile(IFormFile file)
        {
            var tempPath= Path.GetTempPath();
            var fileToSavePath= Path.Combine(tempPath, file.FileName);
            using (var fileStream= System.IO.File.Create(fileToSavePath))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileToSavePath;
        }
    }
}
