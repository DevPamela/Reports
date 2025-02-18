using NPOI.XWPF.UserModel;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;
using System.Collections.Generic;

namespace Reports.Service
{
    public class ReportService : IReportService
    {
        private readonly IFormatterService _formatterService;
        private readonly IStudentService _studentService;
        private readonly IClassroomService _classroomService;
        private readonly ITeacherService _teacherService;
        private readonly IGoalService _goalService;
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssessmentService _assessmentService;
        public ReportService(IFormatterService formatterService, IStudentService studentService, IGoalService goalService, IReportRepository reportRepository,
            IUnitOfWork unitOfWork, IClassroomService classroomService, ITeacherService teacherService, IAssessmentService assessmentService)
        {
            _formatterService = formatterService;
            _studentService = studentService;
            _goalService = goalService;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _classroomService = classroomService;
            _teacherService = teacherService;
            _assessmentService = assessmentService;
        }
        /// <summary>
        /// Saves studentId in the Report entity.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        private async Task SetStudentIdForReport(Report report)
        {
            try
            {
                var studentById = await _studentService.GetByIdAsync(report.StudentId);
                if (studentById != null)
                {
                    report.Student = studentById;
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Create a new instance of report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        private async Task<List<Report>> CreateReportForStudents(Report report, Student student)
        {
            return await Task.Run(() =>
            {
                var reports = new Report
                {
                    FirtsParagraph = report.FirtsParagraph,
                    LastParagraph = report.LastParagraph,
                    Student = student,
                    StudentId = student.Id,
                    Name = report.Name
                };
                var list = new List<Report>
            {
              reports
            };
                return list;
            });
        }

        /// <summary>
        /// Saves report.
        /// </summary>
        /// <param name="reports"></param>
        /// <returns></returns>
        public async Task AddReport(List<Report> reports)
        {
            foreach (var report in reports)
            {
                _reportRepository.Add(report);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task CreateReportAsync(Report report)
        {
            try
            {
                var students = await _studentService.GetStudentByIsActiveAsync();
                foreach (var student in students)
                {
                    var list = await CreateReportForStudents(report, student);
                    report.StudentId = student.Id;
                    await SetStudentIdForReport(report);
                   
                    await CreateReportFile(report);
                }
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }

        /// <summary>
        /// Generates the formatted report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="path"></param>
        private async Task CreateReportFile(Report report)
        {
            try
            {
                XWPFDocument doc = new XWPFDocument();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string reportsFolder = Path.Combine(path, "Relatorios");

                await CreateReportsFolder(reportsFolder);
                await CreateHeader(report.Student, doc);

                if (report.Name != null)
                {
                    await CreateTitle(report, doc);
                }

                if (report.FirtsParagraph != null)
                {
                    await CreateParagraph(report.FirtsParagraph, doc);
                }

                var IsAGirl = report.Student.IsAGirl();

                var assessments = await _assessmentService.GetByStudentIdAsync(report.Student.Id);

                foreach (var a in assessments)
                {
                    try
                    {
                        var studentName = report.Student.Name.Substring(0, 1).ToUpper() + report.Student.Name.Substring(1).ToLower();
                        var name = studentName.Split(' ');
                        var goal = await _goalService.GetGoalByIdAsync(a.GoalId);
                        var goalDescription = await _goalService.GetDescriptionForParagrafAsync(a, report.Student.TeacherId, IsAGirl);

                        await GenerateParagraphsAsync(assessments, doc, report);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }


                if (report.FirtsParagraph != null)
                {
                    await CreateParagraph(report.LastParagraph, doc);
                }

                XWPFParagraph pageBreak = doc.CreateParagraph();
                await SaveDocument(reportsFolder, doc, report.Student.Name);
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }

        public async Task GenerateParagraphsAsync(IEnumerable<Assessment> assessments, XWPFDocument doc, Report report)
        {
            var isAGirl = report.Student.IsAGirl();
            var studentName = CapitalizeFirstName(report.Student.Name);

            foreach (var a in assessments)
            {
                var goal = await _goalService.GetGoalByIdAsync(a.GoalId);
                var goalDescription = await _goalService.GetDescriptionForParagrafAsync(a, report.Student.TeacherId, isAGirl);
                 
                if (!string.IsNullOrEmpty(goalDescription))
                {
                    goalDescription = goalDescription.Replace("{aluno}", studentName);
                }

                await CreateParagraph(goalDescription, doc);
            }
        }

        private string CapitalizeFirstName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))//TODO função de baixar relatório
                return string.Empty;

            var firstName = fullName.Split(' ').FirstOrDefault();
            if (string.IsNullOrEmpty(firstName))
                return string.Empty;

            return firstName.Substring(0, 1).ToUpper() + firstName.Substring(1).ToLower();
        }

        /// <summary>
        /// Creates the paragraphs.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="doc"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        private async Task CreateParagraph(string param, XWPFDocument doc)
        {
            XWPFParagraph paragraph = doc.CreateParagraph();
            XWPFRun run = paragraph.CreateRun();
            run.SetText(param);
            await _formatterService.SetDefaultFormatForReport(paragraph, run, doc);
        }

        /// <summary>
        /// Creates the header.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="doc"></param>
        private async Task CreateParagraphForHeader(string param, XWPFDocument doc)
        {
            XWPFParagraph paragraph = doc.CreateParagraph();
            XWPFRun run = paragraph.CreateRun();
            run.SetText(param);
            await _formatterService.SetDefaultFormatForHeader(run, paragraph);
        }

        /// <summary>
        /// Save the student report as a single Word document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="word"></param>
        /// <param name="path"></param>
        static async Task SaveDocument(string path, XWPFDocument doc, string studentName)
        {
            using (FileStream fs = new FileStream($"{path}\\RELATORIO {studentName.Substring(0)}.docx",
                FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                await Task.Run(() => doc.Write(fs));
            };
        }

        /// <summary>
        /// Create the report title.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private async Task CreateTitle(Report report, XWPFDocument doc)
        {
            var reportTitle = report.Name.ToUpper();
            await CreateParagraphForHeader(reportTitle, doc);
        }

        /// <summary>
        /// Generates the header with students basics informations.
        /// </summary>
        /// <param name="student"></param>
        /// <param name="doc"></param>
        /// <param name="application"></param>
        private async Task CreateHeader(Student student, XWPFDocument doc)
        {
            var classroom = await _classroomService.GetById(student.ClassRoomId);
            var firstLine = classroom.Name;
            var teacher = await _teacherService.GetByIdAsync(student.TeacherId);
            var secondLine = student.Name.ToUpper();
            var absences = student.Absences;
            var thirdLine = $"PROFESSORA: {teacher.Name.ToUpper()}";

            var fourthLine = $"DATA DE NASCIMENTO: {student.Birthdate.ToString("dd/MM/yyyy")}";
            var fifthLine = $"TURMA: {firstLine.ToUpper()}";
            var sixthLine = $"Faltas: {absences}";
            string[] param = { secondLine, thirdLine, fourthLine, fifthLine, sixthLine };

            foreach (var p in param)
            {
                await CreateParagraphForHeader(p, doc);
            }
        }

        /// <summary>
        /// Generates the Report folder.
        /// </summary>
        /// <param name="reportsFolder"></param>
        /// <returns></returns>
        private async Task CreateReportsFolder(string reportsFolder)
        {
            await Task.Run(() =>
           {
               try
               {
                   if (!Directory.Exists(reportsFolder))
                   {
                       Directory.CreateDirectory(reportsFolder);
                   }
               }
               catch
               {
               }
           });
        }

        /// <inheritdoc />
        Task IReportService.GetReportById(Report report)
        {
            throw new NotImplementedException();
        }
    }
}