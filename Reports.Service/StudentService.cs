using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Reports.Domain.Interfaces;
using Reports.Domain.Models;
using Reports.Infrastructure;

namespace Reports.Service
{
    public class StudentService : IStudentService
    {


        public List<Student> AddStudentByList(IFormFile excelFile, string[] param)
        {
            ContextMongoDb db = new ContextMongoDb();

            var sheet1 = new ExcelFileHeaderService(this);
            var students = sheet1.HeadSheet(excelFile, param);
            var report = new Report();
            report.Students = students;

            foreach (var student in students)
            {
                db.Student.InsertOne(student);
            }
            return students.ToList();
        }

        //TODO pensar no banco in memory
        public List<Student> ReadStudentsFromSheet(ISheet sheet, int columnIndexName, int columnIndexAbsence, int columnIndexBirthday)
        {

            // int linesQuantity = sheet.LastRowNum;
            var name = "";
            var absences = "";
            var birthday = "";
            var students = new List<Student>();
            //TODO resolver o index 0 que está pegando errado
            //Como tratar a data de nascimento no report
            //Como passar essa lista para reports?
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    T item = new T();

                    name = GetCellValue(row.GetCell(columnIndexName));
                    absences = GetCellValue(row.GetCell(columnIndexAbsence));
                    birthday = GetCellValue(row.GetCell(columnIndexBirthday));
                    Student student = new Student { Name = name, Absences = absences, Birthday = birthday };
                    students.Add(student);
                }
            }
            return students;
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                return string.Empty;
            }

            return cell.ToString();
        }

        //TODO implementar o banco 
        public List<Student> GetStudentsList()
        {
            ContextMongoDb db = new ContextMongoDb();
            var students = db.Student.Find(s => true).ToList();
            return students;
        }

        public Student GetStudentByName(string name)
        {
            ContextMongoDb db = new ContextMongoDb();
            var student = db.Student.Find(s => s.Name == name).FirstOrDefault();
            return student;
        }

        //TODO pensar em como fazer isso
        //public Student UpdateStudent(Student student)
        //{
        //    var student = classroom.Students.FirstOrDefault(_ => _.Name == nome);
        //}

    }
}