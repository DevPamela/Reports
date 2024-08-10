using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using System.Collections.Generic;
using System.Xml.Linq;
using ICell = NPOI.SS.UserModel.ICell;
using Path = System.IO.Path;

namespace Reports.Service.ExcelFileHeader
{
    public class ExcelFileHeaderService : IExcelFileHeaderService
    {
        public ExcelFileHeaderService()
        {
        }

        public async Task<List<Student>> HeadSheet(IFormFile excelFile)
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
                return await GetColumnIndex(headerRow, sheet);
            }
        }
        /// <summary>
        /// Reads cell values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cell"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<T> GetCellValue<T>(ICell cell)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                return default;
            }

            else if (typeof(T) == typeof(string))
            {
                return (T)(object)cell.ToString();
            }
            else if (typeof(T) == typeof(DateTime))
            {
                if (cell.CellType == CellType.Numeric)
                {
                    DateTime dateTimeValue = DateTime.FromOADate(cell.NumericCellValue);

                    return (T)(object)dateTimeValue;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                throw new ArgumentException("Unsupported data type.");
            }
        }

        /// <summary>
        /// Reads an excel file with student data.
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnIndexName"></param>
        /// <param name="columnIndexBirthday"></param>
        /// <returns></returns>
        public async Task<List<Student>> GetStudentDataFromSheet(ISheet sheet, int columnIndexName, int columnIndexBirthday)
        {
            var name = "";
            DateTime birthday = default;
            var students = new List<Student>();
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    name = await GetCellValue<string>(row.GetCell(columnIndexName));
                    birthday = await GetCellValue<DateTime>(row.GetCell(columnIndexBirthday));
                }
                
                if (name != null)
                {
                    students = await AddStudentFromSheet(students, name, birthday);
                }
            }
                       
            return students;
        }

        public async Task<List<Student>> AddStudentFromSheet(List<Student>students, string name, DateTime birthday)
        {
            Student student = new Student { Name = name, Birthday = birthday };

            students.Add(student);
            
            return students;
        }

        public async Task<List<Student>> GetColumnIndex(IRow headerRow, ISheet sheet)
        {
            int columnIndexName = 0;
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

                            case "data de nascimento":
                                columnIndexBirthday = i;
                                break;
                        }
                    }
                }
            }
            return await GetStudentDataFromSheet(sheet, columnIndexName, columnIndexBirthday);
        }
    }
}