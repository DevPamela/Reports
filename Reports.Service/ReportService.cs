using Reports.Domain.Interfaces.Service;
using DocumentFormat.OpenXml.Packaging;
using Reports.Domain.Models;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Reports.Service
{
    public class ReportService : IReportService
    {
        // private readonly IFormatterService _formatterService;
        public ReportService()
        {
            // _formatterService = formatterService;
        }

        /// <inheritdoc />
        public void CreateReport(Report Report, string path)
        {
            foreach (var r in Report.Students)
            {
                r.IsSatisfatory();
            }
            CreateDocument(Report, path);
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
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(@path, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());



                    foreach (var r in report.Students)
                    {
                        CreateHeader(r, body);
                        CreateParagraph(report.FirtsParagraph, body);
                        // _formatterService.SetDefaultFormatForReport(firstParagraph, word, doc);

                        var IsAGirl = r.IsAGirl();
                        foreach (var g in r.Goals)
                        {
                            string validation = IsAGirl ? "a" : "o";
                            string param = string.Format(g.Name, validation, r.Name);
                            CreateParagraph(param, body);
                            //_formatterService.SetDefaultFormatForReport(paragraph, word, doc);
                        }

                        CreateParagraph(report.LastParagraph, body);
                        // _formatterService.SetDefaultFormatForReport(lastParagraph, word, doc);

                        //doc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);
                        //SaveDocumentAsPdf(doc, word, path);
                        mainPart.Document.Save();
                    }

                }
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
        private void CreateParagraph(string param, Body body)
        {
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text(param));
        }
        /// <summary>
        /// Saves al reports as a single PDF document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="word"></param>
        /// <param name="path"></param>
        //static void SaveDocumentAsPdf(Document doc, Application word, string path)
        //{
        //    // string path = @"D:\Documento.pdf";
        //    doc.SaveAs2(path, WdSaveFormat.wdFormatPDF);
        //    doc.Saved = true;
        //    doc.Close();
        //    word.Quit();
        // }
        /// <summary>
        /// Generates the header with students basics informations.
        /// </summary>
        /// <param name="student"></param>
        /// <param name="doc"></param>
        /// <param name="application"></param>
        private void CreateHeader(Student student, Body body)
        {
            var firstLine = $"Nome: {student.Name}" + "\t" + $"Professora: {student.Teacher.Name}";
            var secondLine = $"Data de nascimento: {student.Birthday}" + "\t" + $"Escola: {student.School.Name}";

            CreateParagraph(firstLine, body);
            CreateParagraph(secondLine, body);



            //    //_formatterService.SetDefaultFormatForHeader(firstParagraph);
            //    //_formatterService.SetDefaultFormatForHeader(secondParagraph);
            //    //_formatterService.SetDefaultFormatForReport(secondParagraph, application, doc);
            //}
        }
    }
}