using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Service
{
    public interface IExcelFileHeaderService
    {
        Task<List<Student>> HeadSheet(IFormFile excelFile);
        Task<T> GetCellValue<T>(ICell cell);
        Task<List<Student>> GetStudentDataFromSheet(ISheet sheet, int columnIndexName, int columnIndexBirthday);
        Task<List<Student>> GetColumnIndex(IRow headerRow, ISheet sheet);
        Task<List<Student>> AddStudentFromSheet(List<Student> students, string name, DateTime birthday);
    }
}