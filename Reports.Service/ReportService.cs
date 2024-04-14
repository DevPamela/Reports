using NPOI.XWPF.UserModel;
using Reports.Domain.Interfaces;
using Reports.Domain.Models;

namespace Reports.Service
{
    public class ReportService : IReportService
    {
        private readonly IFormatterService _formatterService;
        private readonly IStudentService _studentService;
        private readonly IGoalService _goalService;
        public ReportService(IFormatterService formatterService, IStudentService studentService, IGoalService goalService)
        {
            _formatterService = formatterService;
            _studentService = studentService;
            _goalService = goalService;
        }

        /// <inheritdoc />
        public async Task CreateReportAsync(Report report, string path)
        {
            var students = await _studentService.GetStudentsListAsync();

            var teacher = new Teacher
            {
                Name = "Ana"
            };
            var classroom = new ClassRoom
            {
                Name = "Infantil 3"
            };
            //TODO: CHECK LATER
            //var school = new School
            //{
            //    Name = " Colegio Aprendiz"
            //};
            //report.Students = students;
            //foreach (var r in report.Students)
            //{
            //    await _goalService.GetDescriptionForParagrafAsync(r);
            //    r.Teacher = teacher;
            //    r.Class = classroom;
            //    r.School = school;
            //    r.Genre = "F";
            //    //r.IsSatisfatory();
            //}

            CreateDocument(report, path);
        }

        /// <summary>
        /// Generates the formatted report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="path"></param>
        private void CreateDocument(Report report, string path)
        {
            try
            {
                XWPFDocument doc = new XWPFDocument();

                //TODO: CHECK LATER
                //foreach (var r in report.Students)
                //{
                //    CreateHeader(r, doc);
                //    CreateParagraph(report.FirtsParagraph, doc);

                //    var IsAGirl = r.IsAGirl();
                //    foreach (var g in r.Goals)
                //    {
                //        var studentName = r.Name.Substring(0, 1).ToUpper() + r.Name.Substring(1).ToLower();
                //        string validation = IsAGirl ? "a" : "o";
                //        var name = studentName.Split(' ');
                //        string param = string.Format(g.Name, validation, name[0]);
                //        CreateParagraph(param, doc);
                //    }

                //    CreateParagraph(report.LastParagraph, doc);
                //    XWPFParagraph pageBreak = doc.CreateParagraph();
                //    pageBreak.CreateRun().AddBreak(BreakType.PAGE);
                //    SaveDocument(path, doc);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        /// <summary>
        /// Creates the paragraphs.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="doc"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        private void CreateParagraph(string param, XWPFDocument doc)
        {
            XWPFParagraph paragraph = doc.CreateParagraph();
            XWPFRun run = paragraph.CreateRun();
            run.SetText(param);
            _formatterService.SetDefaultFormatForReport(paragraph, run, doc);
        }

        /// <summary>
        /// Creates the header.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="doc"></param>
        private void CreateParagraphForHeader(string param, XWPFDocument doc)
        {
            XWPFParagraph paragraph = doc.CreateParagraph();
            XWPFRun run = paragraph.CreateRun();
            run.SetText(param);
            _formatterService.SetDefaultFormatForHeader(run, paragraph);
        }

        /// <summary>
        /// Saves al reports as a single PDF document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="word"></param>
        /// <param name="path"></param>
        static void SaveDocument(string path, XWPFDocument doc)
        {
            using (FileStream fs = new FileStream($"{path}\\Relatorios.docx", FileMode.Create, FileAccess.Write))
            {
                doc.Write(fs);
            };
        }

        /// <summary>
        /// Generates the header with students basics informations.
        /// </summary>
        /// <param name="student"></param>
        /// <param name="doc"></param>
        /// <param name="application"></param>
        private void CreateHeader(Student student, XWPFDocument doc)
        {
            //TODO: CHECK LATER
            //var firstLine = student.School.Name.ToUpper();
            var secondLine = student.Name.ToUpper();
            var thirdLine = $"PROFESSORA: {student.Teacher.Name.ToUpper()}";
            var fourthLine = $"DATA DE NASCIMENTO: {student.Birthday}";
            var fifthLine = $"TURMA: {student.Class.Name.ToUpper()}";
            string[] param = { /*firstLine,*/ secondLine, thirdLine, fourthLine, fifthLine };

            foreach (var p in param)
            {
                CreateParagraphForHeader(p, doc);
            }
        }
    }
}