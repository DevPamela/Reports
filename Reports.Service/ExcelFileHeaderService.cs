using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Reports.Domain.Interfaces;
using Reports.Domain.Models;
using ICell = NPOI.SS.UserModel.ICell;
using Path = System.IO.Path;

namespace Reports.Service
{
    public class ExcelFileHeaderService
    {
        private readonly IStudentService _studentService;
        public ExcelFileHeaderService(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public List<Student> HeadSheet(IFormFile excelFile, string[] param)
        {
            using (Stream stream = excelFile.OpenReadStream())
            {
                IWorkbook excel;
                if (Path.GetExtension(excelFile.FileName) == ".xlsx")
                {
                    excel = new XSSFWorkbook(stream);
                }
                else
                {
                    excel = new HSSFWorkbook(stream);
                }

                ISheet sheet = excel.GetSheetAt(0);
                var headerRow = sheet.GetRow(0);
                int linesQuantity = sheet.LastRowNum;
                return GetColumnIndex(headerRow, sheet);
            }
        }

        public List<Student> GetColumnIndex(IRow headerRow, ISheet sheet)
        {
            int columnIndexName = 0;
            int columnIndexAbsence = 0;
            int columnIndexBirthday = 0;

            if (headerRow != null)
            {

                for (int i = 0; i < headerRow.LastCellNum; i++)
                {
                    ICell cell = headerRow.GetCell(i);
                    if (cell != null)
                    {
                        string columnName = cell.StringCellValue.ToLower();
                        switch (columnName)
                        {
                            case "nome":
                                columnIndexName = i;
                                break;
                            case "faltas":
                                columnIndexAbsence = i;
                                break;
                            case "data de nascimento":
                                columnIndexBirthday = i;
                                break;
                        }
                    }
                }
            }
            return _studentService.ReadStudentsFromSheet(sheet, columnIndexName, columnIndexAbsence, columnIndexBirthday);
        }
    }
}
