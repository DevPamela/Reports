using NPOI.XWPF.UserModel;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using System.IO.Compression;

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

        /// <inheritdoc />
        public async Task<(byte[] ZipContent, string ZipFileName)> CreateReportAsync(Report report, int teacherId)
        {
            try
            {
                var students = await _studentService.GetStudentByIsActiveAsync(teacherId);


                var reportFile = await CreateReportFile(report, students);
                return reportFile;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Generates the formatted report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="path"></param>
        private async Task<(byte[] ZipContent, string ZipFileName)> CreateReportFile(Report report, List<Student> students)
        {
            try
            {
                var reports = new List<(byte[] Content, string FileName)>();

                foreach (var student in students)
                {
                    var list = await CreateReportForStudents(report, student);
                    report.StudentId = student.Id;
                    await SetStudentIdForReport(report);
                    XWPFDocument doc = new XWPFDocument();

                    await CreateHeader(report.Student, doc);

                    if (report.Name != null)
                    {
                        await CreateTitle(report, doc);
                    }

                    if (report.FirtsParagraph != null)
                    {
                        var studentName = CapitalizeFirstName(report.Student.Name);
                        var paragraphFormated = report.FirtsParagraph.Replace("{aluno} ", studentName + " ").Replace("  ", " ").Trim();
                        await CreateParagraph(paragraphFormated, doc);
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

                            await GenerateParagraphsAsync(a, doc, report);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }

                    if (report.LastParagraph != null)
                    {
                        var studentName = CapitalizeFirstName(report.Student.Name);
                        var paragraphFormated = report.LastParagraph.Replace("{aluno}", studentName + " ").Replace("  ", " ").Trim(); ;
                        await CreateParagraph(paragraphFormated, doc);
                    }

                    XWPFParagraph pageBreak = doc.CreateParagraph();
                    pageBreak.CreateRun().AddBreak(BreakType.PAGE);

                    var reportBytes = await SaveDocument(doc);
                    reports.Add((reportBytes, student.Name + ".docx"));
                }
                using var memoryStream = new MemoryStream();
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var (content, fileName) in reports)
                    {

                        var entry = archive.CreateEntry(fileName);

                        using (var entryStream = entry.Open())
                        {
                            await entryStream.WriteAsync(content, 0, content.Length);
                        }
                    }
                }
                return (memoryStream.ToArray(), $"Relatorios_{DateTime.Now:yyyyMMdd}.zip");
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar o relatório.", e);
            }
        }

        public async Task GenerateParagraphsAsync(Assessment assessment, XWPFDocument doc, Report report)
        {
            var isAGirl = report.Student.IsAGirl();
            var studentName = CapitalizeFirstName(report.Student.Name);

            var goal = await _goalService.GetGoalByIdAsync(assessment.GoalId);
            var goalDescription = await _goalService.GetDescriptionForParagrafAsync(assessment, report.Student.TeacherId, isAGirl);

            if (!string.IsNullOrEmpty(goalDescription))
            {
                goalDescription = goalDescription.Replace("{aluno}", studentName + " ").Replace("  ", " ").Trim(); ;
            }

            await CreateParagraph(goalDescription, doc);
        }

        private string CapitalizeFirstName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
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
        public static async Task<byte[]> SaveDocument(XWPFDocument doc)
        {
            var paragraph = doc.CreateParagraph();
            var run = paragraph.CreateRun();

            using (var exportData = new MemoryStream())
            {
                doc.Write(exportData);
                return exportData.ToArray();
            }
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
            var secondLine = $"ALUNO: {student.Name.ToUpper()}";
            var absences = student.Absences;
            var thirdLine = $"PROFESSORA: {teacher.Name.ToUpper()}";

            var fourthLine = $"DATA DE NASCIMENTO: {student.Birthdate.ToString("dd/MM/yyyy")}";
            var fifthLine = $"TURMA: {firstLine.ToUpper()}";
            var sixthLine = $"FALTAS: {absences}";
            string[] param = { secondLine, thirdLine, fourthLine, fifthLine, sixthLine };

            foreach (var p in param)
            {
                await CreateParagraphForHeader(p, doc);
            }
        }
    }
}