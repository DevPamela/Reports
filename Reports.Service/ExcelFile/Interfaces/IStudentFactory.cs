using NPOI.SS.UserModel;
using Reports.Domain.Models;

namespace Reports.Service.ExcelFile.Interfaces;
public interface IStudentFactory
{
    Student CreateStudent(IRow row);
}